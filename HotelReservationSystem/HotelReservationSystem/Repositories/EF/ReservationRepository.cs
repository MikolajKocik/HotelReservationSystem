using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Repositories.EF
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HotelDbContext _context;

        public ReservationRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(g => g.Guest)
                .ToListAsync();
        }

        public async Task<Reservation> GetById(int id)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(g => g.Guest)
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new InvalidOperationException("Reservation not found");
        }

        public async Task Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }
    }
}
