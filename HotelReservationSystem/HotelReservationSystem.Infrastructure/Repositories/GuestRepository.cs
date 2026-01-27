using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for guest entities
/// </summary>
public class GuestRepository : IGuestRepository
{
    private readonly HotelDbContext context;

    public GuestRepository(HotelDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets all guests with pagination and filtering support
    /// </summary>
    public async Task<IQueryable<Guest>> GetAllAsync()
        => await Task.FromResult(context.Guests
                .AsNoTracking()
                .Include(g => g.Reservations));

    /// <summary>
    /// Gets a guest by their unique identifier
    /// </summary>
    public async Task<Guest?> GetByIdAsync(string id)
        => await context.Guests
                .Include(g => g.Reservations)
                .FirstOrDefaultAsync(g => g.Id == id);

    /// <summary>
    /// Gets a guest by their email address
    /// </summary>
    public async Task<Guest?> GetByEmailAsync(string email)
        => await context.Guests
            .Include(g => g.Reservations)
            .FirstOrDefaultAsync(g => g.Email == email);

    /// <summary>
    /// Creates a new guest
    /// </summary>
    public async Task<string> CreateAsync(Guest guest)
    {
        context.Guests.Add(guest);
        await context.SaveChangesAsync();
        return guest.Id;
    }

    /// <summary>
    /// Updates an existing guest
    /// </summary>
    public async Task UpdateAsync(Guest guest)
    {
        context.Guests.Update(guest);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a guest
    /// </summary>
    public async Task DeleteAsync(string id)
    {
        var guest = await GetByIdAsync(id);
        if (guest != null)
        {
            context.Guests.Remove(guest);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Gets payment transactions for reporting
    /// </summary>
    public async Task<List<Payment>> GetTransactions()
        => await context.Payments
            .Include(p => p.Reservation)
            .ThenInclude(r => r.Guest)
            .ToListAsync();
}

