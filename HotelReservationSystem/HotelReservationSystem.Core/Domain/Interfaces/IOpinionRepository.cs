using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Core.Domain.Interfaces;

public interface IOpinionRepository
{
    Task<Opinion?> GetByIdAsync(string id);
    Task<Opinion?> GetByReservationIdAsync(string reservationId);
    Task<IEnumerable<Opinion>> GetAllAsync();
    Task<IEnumerable<Opinion>> GetByGuestIdAsync(string guestId);
    Task AddAsync(Opinion opinion);
    Task UpdateAsync(Opinion opinion);
    Task DeleteAsync(string id);
}