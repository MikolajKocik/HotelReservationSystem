using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Core.Domain.Interfaces;
    /// <summary>
    /// Repository interface for managing room entities
    /// </summary>
    public interface IRoomRepository
    {
        /// <summary>
        /// Gets all rooms with related data
        /// </summary>
        Task<IQueryable<Room>> GetAllAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a room by its unique identifier
        /// </summary>
        Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets available rooms for a date range
        /// </summary>
        Task<IQueryable<Room>> GetAvailableRoomsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets rooms by type with availability status for date range
        /// </summary>
        Task<IQueryable<Room>> GetRoomsByTypeAsync(string? roomType, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates a new room
        /// </summary>
        Task<int> CreateAsync(Room room, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates an existing room
        /// </summary>
        Task UpdateAsync(Room room, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Deletes a room
        /// </summary>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Toggles room availability status
        /// </summary>
        Task ToggleAvailabilityAsync(int id, CancellationToken cancellationToken = default);
    }
