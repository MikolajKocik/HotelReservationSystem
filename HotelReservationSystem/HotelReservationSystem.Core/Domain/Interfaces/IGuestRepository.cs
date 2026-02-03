using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Core.Domain.Interfaces;

/// <summary>
/// Repository interface for managing guest entities
/// </summary>
public interface IGuestRepository
{
    /// <summary>
    /// Gets all guests with pagination and filtering support
    /// </summary>
    Task<IEnumerable<Guest>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a guest by their unique identifier
    /// </summary>
    Task<Guest?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a guest by their email address
    /// </summary>
    Task<Guest?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new guest
    /// </summary>
    Task<string> CreateAsync(Guest guest, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing guest
    /// </summary>
    Task UpdateAsync(Guest guest, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a guest
    /// </summary>
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets payment transactions for reporting
    /// </summary>
    Task<List<Payment>> GetTransactions(CancellationToken cancellationToken = default);
}
