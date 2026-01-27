using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Core.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for managing room entities
    /// </summary>
    public interface IRoomRepository
    {
        /// <summary>
        /// Gets all rooms with related data
        /// </summary>
        Task<IQueryable<Room>> GetAllAsync();
        
        /// <summary>
        /// Gets a room by its unique identifier
        /// </summary>
        Task<Room?> GetByIdAsync(int id);
        
        /// <summary>
        /// Gets available rooms for a date range
        /// </summary>
        Task<IQueryable<Room>> GetAvailableRoomsAsync(DateTime from, DateTime to);
        
        /// <summary>
        /// Creates a new room
        /// </summary>
        Task<int> CreateAsync(Room room);
        
        /// <summary>
        /// Updates an existing room
        /// </summary>
        Task UpdateAsync(Room room);
        
        /// <summary>
        /// Deletes a room
        /// </summary>
        Task DeleteAsync(int id);
        
        /// <summary>
        /// Toggles room availability status
        /// </summary>
        Task ToggleAvailabilityAsync(int id);
    }
}
