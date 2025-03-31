using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Repositories.EF
{
    public class GuestRepository : IGuestRepository
    {

        private readonly HotelDbContext _context;

        public GuestRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetTransactions() 
            => await _context.Payments
                .Include(p => p.Reservation)
                .ThenInclude(r => r.Guest)
                .ToListAsync();
    }
}
