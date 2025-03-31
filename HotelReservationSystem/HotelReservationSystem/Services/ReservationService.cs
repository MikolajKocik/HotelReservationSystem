using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.ViewModels;
using HotelReservationSystem.Repositories.Interfaces;
using HotelReservationSystem.Services.Interfaces;

namespace HotelReservationSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        public ReservationService(IReservationRepository reservationRepository,
            IRoomRepository roomRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
        }
        public async Task CancelReservation(int id, string reason)
        {
            var reservation = await _reservationRepository.GetById(id);
            reservation.Status = "Anulowana";
            reservation.Reason = "Anulowana przez " + reason;
            await _reservationRepository.Update(reservation);
        }

        public async Task ConfirmReservation(int id)
        {
            var reservation = await _reservationRepository.GetById(id);
            reservation.Status = "Potwierdzona";
            await _reservationRepository.Update(reservation);
        }

        public async Task<int> CreateReservation(ReservationViewModel model)
        {
            var reservation = new Reservation
            {
                ArrivalDate = model.ArrivalDate,
                DepartureDate = model.DepartureDate,
                Status = "Oczekuje",
                RoomId = model.RoomId,
                Reason = "",
                Guest = new Guest
                {
                    FirstName = model.GuestFirstName,
                    LastName = model.GuestLastName,
                    Email = model.GuestEmail,
                    PhoneNumber = model.GuestPhoneNumber
                }
            };
            await _reservationRepository.Add(reservation);
            return reservation.Id;
        }     
    }
}
