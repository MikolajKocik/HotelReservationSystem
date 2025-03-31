using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Repositories.EF
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _context;

        public RoomRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime from, DateTime to)
        {
            return await _context.Rooms
                .Where(r => r.IsAvailable)
                .ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Room>> GetAll()
        {
            return await _context.Rooms.ToListAsync();
        }

    }
}
