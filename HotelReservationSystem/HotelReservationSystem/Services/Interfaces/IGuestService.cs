using HotelReservationSystem.Models.Domain;

namespace HotelReservationSystem.Services.Interfaces
{
    public interface IGuestService
    {
        Task UpdateGuestAsync(int guestId, Guest updatedData);
        Task<Guest> GetGuestByIdAsync(int id);
    }

}
