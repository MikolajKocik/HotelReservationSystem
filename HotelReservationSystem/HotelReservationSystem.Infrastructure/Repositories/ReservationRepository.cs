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
    public async Task<IEnumerable<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
        => await this.context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .ToListAsync(cancellationToken);

    /// <summary>
    /// Gets a reservation by its unique identifier
    /// </summary>
    public async Task<Reservation?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        Reservation? reservation = await this.context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return reservation;
    }


    /// <summary>
    /// Gets reservations within a specific date range 
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        => await this.context.Reservations
            .AsNoTracking()
            .Include(r => r.Room)
            .Include(r => r.Guest)
            .Include(r => r.Payment)
            .Where(r => r.ArrivalDate <= to && r.DepartureDate >= from)
            .ToListAsync(cancellationToken);


    /// <summary>
    /// Gets reservations for a specific guest by email
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetByGuestEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        Guest? guest = await this.context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Email == email, cancellationToken);

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
            INNER JOIN AspNetUsers g ON r.GuestId = g.Id
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
    public async Task<string> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        this.context.Reservations.Add(reservation);
        await this.context.SaveChangesAsync(cancellationToken);

        InvalidateCache(reservation);

        return reservation.Id;
    }

    /// <summary>
    /// Updates an existing reservation
    /// </summary>
    public async Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {

        var entry = this.context.Entry(reservation);
        if (entry.State == EntityState.Detached)
        {
            this.context.Reservations.Update(reservation);
        }
        
        await this.context.SaveChangesAsync(cancellationToken);
        InvalidateCache(reservation);
    }

    /// <summary>
    /// Deletes a reservation
    /// </summary>
    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        Reservation? reservation = await this.context.Reservations.FindAsync(new object[] { id }, cancellationToken);
        if (reservation != null)
        {
            this.context.Reservations.Remove(reservation);
            await this.context.SaveChangesAsync(cancellationToken);

            this.cache.Remove($"Reservation_{id}");
            InvalidateCache(reservation);
        }
    }

    /// <summary>
    /// Gets reservations for a specific room within a date range
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetByRoomAndDateRangeAsync(int roomId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
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
                .ToListAsync(cancellationToken);

        this.cache.Set(key, list, TimeSpan.FromMinutes(2));
        return list.Where(r => r.ArrivalDate < to && r.DepartureDate > from);
    }

    /// <summary>
    /// Gets all guests for lookup purposes
    /// </summary>
    public async Task<List<Guest>> GetGuestsAsync(CancellationToken cancellationToken = default)
        => await this.context.Users
                .Include(g => g.Reservations)
                .ToListAsync(cancellationToken);

    private void InvalidateCache(Reservation reservation)
    {
        this.cache.Remove($"Reservations_Guest_{reservation.GuestId}");
        this.cache.Remove("AvailableRooms");
        this.cache.Remove($"Reservations_Room_{reservation.RoomId}");
    }
}


