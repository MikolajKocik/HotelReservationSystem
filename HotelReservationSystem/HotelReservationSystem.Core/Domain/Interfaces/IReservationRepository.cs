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
        Task<IEnumerable<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a reservation by its unique identifier
        /// </summary>
        Task<Reservation?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets reservations within a specific date range
        /// </summary>
        Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets reservations for a specific guest by email
        /// </summary>
        Task<IEnumerable<Reservation>> GetByGuestEmailAsync(string email, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates a new reservation
        /// </summary>
        Task<string> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates an existing reservation
        /// </summary>
        Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Deletes a reservation
        /// </summary>
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets reservations for a specific room within a date range
        /// </summary>
        Task<IEnumerable<Reservation>> GetByRoomAndDateRangeAsync(int roomId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets all guests for lookup purposes
        /// </summary>
        Task<List<Guest>> GetGuestsAsync(CancellationToken cancellationToken = default);
    }
