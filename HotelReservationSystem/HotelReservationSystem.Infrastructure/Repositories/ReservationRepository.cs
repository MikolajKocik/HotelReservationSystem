using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for reservation entities
/// </summary>
public class ReservationRepository : IReservationRepository
{
    private readonly HotelDbContext context;

    public ReservationRepository(HotelDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets all reservations with related entities
    /// </summary>
    public async Task<IQueryable<Reservation>> GetAllAsync()
        => await Task.FromResult(context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment));


    /// <summary>
    /// Gets a reservation by its unique identifier
    /// </summary>
    public async Task<Reservation?> GetByIdAsync(string id)
        => await context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(r => r.Id == id);


    /// <summary>
    /// Gets reservations within a specific date range
    /// </summary>
    public async Task<IQueryable<Reservation>> GetByDateRangeAsync(DateTime from, DateTime to)
        => await Task.FromResult(context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .Where(r => r.ArrivalDate >= from && r.DepartureDate <= to));


    /// <summary>
    /// Gets reservations for a specific guest by email
    /// </summary>
    public async Task<IQueryable<Reservation>> GetByGuestEmailAsync(string email)
        => await Task.FromResult(context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .Where(r => r.Guest.Email == email));


    /// <summary>
    /// Creates a new reservation
    /// </summary>
    public async Task<string> CreateAsync(Reservation reservation)
    {
        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();
        return reservation.Id;
    }

    /// <summary>
    /// Updates an existing reservation
    /// </summary>
    public async Task UpdateAsync(Reservation reservation)
    {
        context.Reservations.Update(reservation);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a reservation
    /// </summary>
    public async Task DeleteAsync(string id)
    {
        Reservation? reservation = await GetByIdAsync(id);
        if (reservation != null)
        {
            context.Reservations.Remove(reservation);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Gets reservations for a specific room within a date range
    /// </summary>
    public async Task<IQueryable<Reservation>> GetByRoomAndDateRangeAsync(int roomId, DateTime from, DateTime to)
        => await Task.FromResult(context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Include(r => r.Payment)
                .Where(r => r.RoomId == roomId && 
                           ((r.ArrivalDate <= to && r.DepartureDate >= from))));

    /// <summary>
    /// Gets all guests for lookup purposes
    /// </summary>
    public async Task<List<Guest>> GetGuestsAsync()
        => await context.Guests
                .Include(g => g.Reservations)
                .ToListAsync();

}

