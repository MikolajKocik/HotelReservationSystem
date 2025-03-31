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
    }
}
