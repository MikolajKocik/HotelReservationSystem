using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Application.UseCases
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository guestRepository;

        public GuestService(IGuestRepository guestRepo)
        {
            guestRepository = guestRepo;
        }
    }
}