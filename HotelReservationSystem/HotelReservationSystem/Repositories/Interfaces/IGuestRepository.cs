using HotelReservationSystem.Models.Domain;

namespace HotelReservationSystem.Repositories.Interfaces
{
    public interface IGuestRepository
    {
        Task<Guest> GetByIdAsync(int id);
        Task UpdateAsync(Guest guest);
    }

}
