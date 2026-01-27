using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for room entities
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext context;

        public RoomRepository(HotelDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets all rooms with related data
        /// </summary>
        public async Task<IQueryable<Room>> GetAllAsync()
        {
            return await Task.FromResult(context.Rooms
                .AsNoTracking()
                .Include(r => r.Reservations));
        }

        /// <summary>
        /// Gets a room by its unique identifier
        /// </summary>
        public async Task<Room?> GetByIdAsync(int id)
        {
            return await context.Rooms
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Gets available rooms for a date range
        /// </summary>
        public async Task<IQueryable<Room>> GetAvailableRoomsAsync(DateTime from, DateTime to)
        {
            return await Task.FromResult(context.Rooms
                .AsNoTracking()
                .Where(r => r.IsAvailable && 
                           !r.Reservations.Any(res => 
                               res.ArrivalDate < to && res.DepartureDate > from)));
        }

        /// <summary>
        /// Creates a new room
        /// </summary>
        public async Task<int> CreateAsync(Room room)
        {
            context.Rooms.Add(room);
            await context.SaveChangesAsync();
            return room.Id;
        }

        /// <summary>
        /// Updates an existing room
        /// </summary>
        public async Task UpdateAsync(Room room)
        {
            context.Rooms.Update(room);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a room
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var room = await GetByIdAsync(id);
            if (room != null)
            {
                context.Rooms.Remove(room);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Toggles room availability status
        /// </summary>
        public async Task ToggleAvailabilityAsync(int id)
        {
            var room = await GetByIdAsync(id);
            if (room != null)
            {
                room.SetAvailability(!room.IsAvailable);
                await UpdateAsync(room);
            }
        }
    }
}
