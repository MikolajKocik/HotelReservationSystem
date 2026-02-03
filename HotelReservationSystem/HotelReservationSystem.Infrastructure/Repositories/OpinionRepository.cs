using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Repositories;

public sealed class OpinionRepository : IOpinionRepository
{
    private readonly HotelDbContext context;

    public OpinionRepository(HotelDbContext context)
    {
        this.context = context;
    }

    public async Task<Opinion?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        => await this.context.Opinions
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    

    public async Task<Opinion?> GetByReservationIdAsync(string reservationId, CancellationToken cancellationToken = default)
        => await this.context.Opinions
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .FirstOrDefaultAsync(o => o.ReservationId == reservationId, cancellationToken);
    

    public async Task<IEnumerable<Opinion>> GetAllAsync(CancellationToken cancellationToken = default)
        => await this.context.Opinions
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .ToListAsync(cancellationToken);
    
    public async Task<IEnumerable<Opinion>> GetByGuestIdAsync(string guestId, CancellationToken cancellationToken = default)
        => await this.context.Opinions
                .Where(o => o.GuestId == guestId)
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .ToListAsync(cancellationToken);
    
    public async Task AddAsync(Opinion opinion, CancellationToken cancellationToken = default)
    {
        await this.context.Opinions.AddAsync(opinion, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Opinion opinion, CancellationToken cancellationToken = default)
    {
        this.context.Opinions.Update(opinion);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        Opinion? opinion = await GetByIdAsync(id, cancellationToken);
        if (opinion != null)
        {
            this.context.Opinions.Remove(opinion);
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}