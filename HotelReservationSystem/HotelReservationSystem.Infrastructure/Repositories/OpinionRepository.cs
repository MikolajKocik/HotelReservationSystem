using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Repositories;

public class OpinionRepository : IOpinionRepository
{
    private readonly HotelDbContext _context;

    public OpinionRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Opinion?> GetByIdAsync(string id)
        => await _context.Opinions
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .FirstOrDefaultAsync(o => o.Id == id);
    

    public async Task<Opinion?> GetByReservationIdAsync(string reservationId)
        => await _context.Opinions
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .FirstOrDefaultAsync(o => o.ReservationId == reservationId);
    

    public async Task<IEnumerable<Opinion>> GetAllAsync()
        => await _context.Opinions
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .ToListAsync();
    
    public async Task<IEnumerable<Opinion>> GetByGuestIdAsync(string guestId)
        => await _context.Opinions
                .Where(o => o.GuestId == guestId)
                .Include(o => o.Reservation)
                .Include(o => o.Guest)
                .ToListAsync();
    
    public async Task AddAsync(Opinion opinion)
    {
        await _context.Opinions.AddAsync(opinion);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Opinion opinion)
    {
        _context.Opinions.Update(opinion);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        Opinion? opinion = await GetByIdAsync(id);
        if (opinion != null)
        {
            _context.Opinions.Remove(opinion);
            await _context.SaveChangesAsync();
        }
    }
}