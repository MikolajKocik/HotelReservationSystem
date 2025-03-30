using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Repositories.Interfaces;
using HotelReservationSystem.Services.Interfaces;

namespace HotelReservationSystem.Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;

        public GuestService(IGuestRepository guestRepo)
        {
            _guestRepository = guestRepo;
        }

        public async Task UpdateGuestAsync(int guestId, Guest updated)
        {
            var guest = await _guestRepository.GetByIdAsync(guestId);
            guest.FirstName = updated.FirstName;
            guest.PhoneNumber = updated.PhoneNumber;
            await _guestRepository.UpdateAsync(guest);
        }

        public async Task<Guest> GetGuestByIdAsync(int id)
        {
            return await _guestRepository.GetByIdAsync(id);
        }
    }

}
