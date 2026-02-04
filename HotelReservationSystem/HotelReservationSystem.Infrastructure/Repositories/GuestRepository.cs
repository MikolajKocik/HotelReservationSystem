using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace HotelReservationSystem.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for guest entities
/// </summary>
public sealed class GuestRepository : IGuestRepository
{
    private readonly HotelDbContext context;

    public GuestRepository(HotelDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets all guests with pagination and filtering support
    /// </summary>
    public async Task<IEnumerable<Guest>> GetAllAsync(CancellationToken cancellationToken = default)
        => await this.context.Users
            .AsNoTracking()
            .Include(g => g.Reservations)
                .ThenInclude(r => r.Room)
            .Include(g => g.Reservations)
                .ThenInclude(r => r.Payment)
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Gets a guest by their unique identifier
    /// </summary>
    public async Task<Guest?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        => await this.context.Users
                .Include(g => g.Reservations)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    /// <summary>
    /// Gets a guest by their email address
    /// </summary>
    public async Task<Guest?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await this.context.Users
            .Include(g => g.Reservations)
            .FirstOrDefaultAsync(g => g.Email == email, cancellationToken);

    /// <summary>
    /// Creates a new guest
    /// </summary>
    public async Task<string> CreateAsync(Guest guest, CancellationToken cancellationToken = default)
    {
        this.context.Users.Add(guest);
        await this.context.SaveChangesAsync(cancellationToken);
        return guest.Id;
    }

    /// <summary>
    /// Updates an existing guest
    /// </summary>
    public async Task UpdateAsync(Guest guest, CancellationToken cancellationToken = default)
    {
        this.context.Users.Update(guest);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a guest
    /// </summary>
    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var guest = await GetByIdAsync(id, cancellationToken);
        if (guest != null)
        {
            this.context.Users.Remove(guest);
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Gets payment transactions for reporting
    /// </summary>
    public async Task<List<Payment>> GetTransactions(CancellationToken cancellationToken = default)
    {
        return await this.context.Payments
            .Include(p => p.Reservation)
            .ThenInclude(r => r.Guest)
            .ToListAsync(cancellationToken);
    }
}
