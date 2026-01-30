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

    public async Task<Opinion?> GetByIdAsync(string id)
        => await this.context.Opinions
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .FirstOrDefaultAsync(o => o.Id == id);
    

    public async Task<Opinion?> GetByReservationIdAsync(string reservationId)
        => await this.context.Opinions
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .FirstOrDefaultAsync(o => o.ReservationId == reservationId);
    

    public async Task<IEnumerable<Opinion>> GetAllAsync()
        => await this.context.Opinions
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .ToListAsync();
    
    public async Task<IEnumerable<Opinion>> GetByGuestIdAsync(string guestId)
        => await this.context.Opinions
                .Where(o => o.GuestId == guestId)
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .ToListAsync();
    
    public async Task AddAsync(Opinion opinion)
    {
        await this.context.Opinions.AddAsync(opinion);
        await this.context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Opinion opinion)
    {
        this.context.Opinions.Update(opinion);
        await this.context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        Opinion? opinion = await GetByIdAsync(id);
        if (opinion != null)
        {
            this.context.Opinions.Remove(opinion);
            await this.context.SaveChangesAsync();
        }
    }
}