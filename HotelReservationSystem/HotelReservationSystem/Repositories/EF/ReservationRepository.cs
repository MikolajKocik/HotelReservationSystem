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

        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }

        public async IEnumerable<Reservation> GetAll()      
           => _context.Reservations
                .Include(r => r.Room)
                .Include(g => g.Guest)
                .ToList();
        

        public async Reservation GetById(int id)
            => _context.Reservations
                .Include(r => r.Room)
                .Include(g => g.Guest)
                .FirstOrDefault(r => r.Id == id);

        public void Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            _context.SaveChanges();
        }
    }
}
