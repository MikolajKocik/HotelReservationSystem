using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Dapper;

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
    public async Task<IQueryable<Reservation>> GetAllAsync()
        => await Task.FromResult(this.context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment));


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
        {
            this.cache.Set(key, reservation, TimeSpan.FromSeconds(60));
        }

        return reservation;
    }


    /// <summary>
    /// Gets reservations within a specific date range
    /// </summary>
    public async Task<IQueryable<Reservation>> GetByDateRangeAsync(DateTime from, DateTime to)
        => await Task.FromResult(this.context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .Where(r => r.ArrivalDate >= from && r.DepartureDate <= to));


    /// <summary>
    /// Gets reservations for a specific guest by email
    /// </summary>
    public async Task<IQueryable<Reservation>> GetByGuestEmailAsync(string email)
    {
        string key = $"Reservations_Guest_{email}";
        if (this.cache.TryGetValue(key, out List<Reservation>? cachedList))
            return await Task.FromResult(cachedList.AsQueryable());

        using var conn = this.context.Database.GetDbConnection();
        
        string sql = @"
            SELECT 
                r.*, 
                rm.*, 
                g.*, 
                p.*
            FROM Reservations r
            INNER JOIN Rooms rm ON r.RoomId = rm.Id
            INNER JOIN Guests g ON r.GuestId = g.Id
            LEFT JOIN Payments p ON r.PaymentId = p.Id
            WHERE g.Email = @Email";

        var reservationsMap = await conn.QueryAsync<Reservation, Room, Guest, Payment, Reservation>(
            sql,
            (res, room, guest, payment) =>
            {
                res.Room = room;
                res.Guest = guest;
                res.Payment = payment;
                return res;
            },
            new { Email = email },
            splitOn: "Id,Id,Id" 
        );

        List<Reservation> rows = reservationsMap.ToList();
        
        this.cache.Set(key, rows, TimeSpan.FromSeconds(30));
        return rows.AsQueryable();
    }

    /// <summary>
    /// Creates a new reservation
    /// </summary>
    public async Task<string> CreateAsync(Reservation reservation)
    {
        this.context.Reservations.Add(reservation);
        await this.context.SaveChangesAsync();

        this.cache.Remove($"Reservations_Guest_{reservation.GuestId}");
        this.cache.Remove("AvailableRooms");

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
        this.cache.Remove($"Reservations_Guest_{reservation.GuestId}");

        this.cache.Remove("AvailableRooms");
    }

    /// <summary>
    /// Deletes a reservation
    /// </summary>
    public async Task DeleteAsync(string id)
    {
        Reservation? reservation = await GetByIdAsync(id);
        if (reservation != null)
        {
            this.context.Reservations.Remove(reservation);
            await this.context.SaveChangesAsync();

            this.cache.Remove($"Reservation_{id}");
            this.cache.Remove($"Reservations_Guest_{reservation.GuestId}");

            this.cache.Remove("AvailableRooms");
        }
    }

    /// <summary>
    /// Gets reservations for a specific room within a date range
    /// </summary>
    public async Task<IQueryable<Reservation>> GetByRoomAndDateRangeAsync(int roomId, DateTime from, DateTime to)
    {
        string key = $"Reservations_Room_{roomId}_{from.Ticks}_{to.Ticks}";
        if (this.cache.TryGetValue(key, out List<Reservation>? cachedList))
            return await Task.FromResult(cachedList.AsQueryable());

        List<Reservation> list = await this.context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .Where(r => r.RoomId == roomId && 
                    ((r.ArrivalDate < to && r.DepartureDate > from)))
                .ToListAsync();

        this.cache.Set(key, list, TimeSpan.FromSeconds(20));
        return list.AsQueryable();
    }

    /// <summary>
    /// Gets all guests for lookup purposes
    /// </summary>
    public async Task<List<Guest>> GetGuestsAsync()
        => await this.context.Guests
                .Include(g => g.Reservations)
                .ToListAsync();

}


