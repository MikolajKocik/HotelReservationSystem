using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Core.Domain.Interfaces;

public interface IOpinionRepository
{
    Task<Opinion?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Opinion?> GetByReservationIdAsync(string reservationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Opinion>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Opinion>> GetByGuestIdAsync(string guestId, CancellationToken cancellationToken = default);
    Task AddAsync(Opinion opinion, CancellationToken cancellationToken = default);
    Task UpdateAsync(Opinion opinion, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}