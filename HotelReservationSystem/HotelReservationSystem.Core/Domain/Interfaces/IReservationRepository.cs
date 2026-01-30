using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Core.Domain.Interfaces;
    /// <summary>
    /// Repository interface for managing reservation entities
    /// </summary>
    public interface IReservationRepository
    {
        /// <summary>
        /// Gets all reservations with related entities
        /// </summary>
        Task<IQueryable<Reservation>> GetAllAsync();
        
        /// <summary>
        /// Gets a reservation by its unique identifier
        /// </summary>
        Task<Reservation?> GetByIdAsync(string id);
        
        /// <summary>
        /// Gets reservations within a specific date range
        /// </summary>
        Task<IQueryable<Reservation>> GetByDateRangeAsync(DateTime from, DateTime to);
        
        /// <summary>
        /// Gets reservations for a specific guest by email
        /// </summary>
        Task<IQueryable<Reservation>> GetByGuestEmailAsync(string email);
        
        /// <summary>
        /// Creates a new reservation
        /// </summary>
        Task<string> CreateAsync(Reservation reservation);
        
        /// <summary>
        /// Updates an existing reservation
        /// </summary>
        Task UpdateAsync(Reservation reservation);
        
        /// <summary>
        /// Deletes a reservation
        /// </summary>
        Task DeleteAsync(string id);
        
        /// <summary>
        /// Gets reservations for a specific room within a date range
        /// </summary>
        Task<IQueryable<Reservation>> GetByRoomAndDateRangeAsync(int roomId, DateTime from, DateTime to);
        
        /// <summary>
        /// Gets all guests for lookup purposes
        /// </summary>
        Task<List<Guest>> GetGuestsAsync();
    }
