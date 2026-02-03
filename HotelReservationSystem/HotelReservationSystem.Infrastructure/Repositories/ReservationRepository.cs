using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Dapper;
using Microsoft.Data.SqlClient;

namespace HotelReservationSystem.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for reservation entities with simple caching
/// </summary>
public sealed class ReservationRepository : IReservationRepository
{
    private readonly HotelDbContext context;
    private readonly IMemoryCache cache;

    public ReservationRepository(HotelDbContext context, IMemoryCache cache)
    {
        this.context = context;
        this.cache = cache;
    }

    /// <summary>
    /// Gets all reservations with related entities
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetAllAsync()
        => await this.context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .ToListAsync();

    /// <summary>
    /// Gets a reservation by its unique identifier
    /// </summary>
    public async Task<Reservation?> GetByIdAsync(string id)
    {
        string key = $"Reservation_{id}";
        if (this.cache.TryGetValue(key, out Reservation? cached))
            return await Task.FromResult(cached);

        Reservation? reservation = await this.context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(r => r.Id == id);

        if (reservation != null)   
            this.cache.Set(key, reservation, TimeSpan.FromSeconds(60));
        

        return reservation;
    }


    /// <summary>
    /// Gets reservations within a specific date range
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime from, DateTime to)
        => await this.context.Reservations
            .AsNoTracking()
            .Include(r => r.Room)
            .Include(r => r.Guest)
            .Include(r => r.Payment)
            .Where(r => r.ArrivalDate >= from && r.DepartureDate <= to)
            .ToListAsync();


    /// <summary>
    /// Gets reservations for a specific guest by email
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetByGuestEmailAsync(string email)
    {
        Guest? guest = await this.context.Guests
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Email == email);

        if (guest == null)
            return Enumerable.Empty<Reservation>();

        string key = $"Reservations_Guest_{guest.Id}";
        if (this.cache.TryGetValue(key, out var cached) && cached is List<Reservation> cachedList)
            return cachedList;

        string? connectionString = this.context.Database.GetConnectionString();
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        await using var conn = new SqlConnection(connectionString);

        string sql = @"
            SELECT r.*, rm.*, g.*, p.*
            FROM Reservations r
            INNER JOIN Rooms rm ON r.RoomId = rm.Id
            INNER JOIN Guests g ON r.GuestId = g.Id
            LEFT JOIN Payments p ON r.PaymentId = p.Id
            WHERE g.Id = @GuestId";

        var result = await conn.QueryAsync<Reservation, Room, Guest, Payment, Reservation>(
            sql,
            (res, room, guest, payment) => {
                res.Room = room;
                res.Guest = guest;
                res.Payment = payment;
                return res;
            },
            new { GuestId = guest.Id },
            splitOn: "Id,Id,Id" 
        );

        var rows = result.ToList();
        this.cache.Set(key, rows, TimeSpan.FromMinutes(10));
        return rows;
    }

    /// <summary>
    /// Creates a new reservation
    /// </summary>
    public async Task<string> CreateAsync(Reservation reservation)
    {
        this.context.Reservations.Add(reservation);
        await this.context.SaveChangesAsync();

        InvalidateCache(reservation);

        return reservation.Id;
    }

    /// <summary>
    /// Updates an existing reservation
    /// </summary>
    public async Task UpdateAsync(Reservation reservation)
    {
        this.context.Reservations.Update(reservation);
        await this.context.SaveChangesAsync();

        this.cache.Remove($"Reservation_{reservation.Id}");
        InvalidateCache(reservation);
    }

    /// <summary>
    /// Deletes a reservation
    /// </summary>
    public async Task DeleteAsync(string id)
    {
        Reservation? reservation = await this.context.Reservations.FindAsync(id);
        if (reservation != null)
        {
            this.context.Reservations.Remove(reservation);
            await this.context.SaveChangesAsync();

            this.cache.Remove($"Reservation_{id}");
            InvalidateCache(reservation);
        }
    }

    /// <summary>
    /// Gets reservations for a specific room within a date range
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetByRoomAndDateRangeAsync(int roomId, DateTime from, DateTime to)
    {
        string key = $"Reservations_Room_{roomId}";
        if (this.cache.TryGetValue(key, out var cached) && cached is List<Reservation> cachedList)
            return cachedList.Where(r => r.ArrivalDate < to && r.DepartureDate > from);

        List<Reservation> list = await this.context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .Where(r => r.RoomId == roomId)
                .ToListAsync();

        this.cache.Set(key, list, TimeSpan.FromMinutes(2));
        return list.Where(r => r.ArrivalDate < to && r.DepartureDate > from);
    }

    /// <summary>
    /// Gets all guests for lookup purposes
    /// </summary>
    public async Task<List<Guest>> GetGuestsAsync()
        => await this.context.Guests
                .Include(g => g.Reservations)
                .ToListAsync();

    private void InvalidateCache(Reservation reservation)
    {
        this.cache.Remove($"Reservations_Guest_{reservation.GuestId}");
        this.cache.Remove("AvailableRooms");
        this.cache.Remove($"Reservations_Room_{reservation.RoomId}");
    }
}


