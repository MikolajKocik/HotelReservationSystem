using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.UseCases
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IRoomRepository roomRepository;
        
        public ReservationService(IReservationRepository reservationRepository,
            IRoomRepository roomRepository)
        {
            this.reservationRepository = reservationRepository;
            this.roomRepository = roomRepository;
        }
        
        public async Task CancelReservation(string id, string reason)
        {
            var reservation = await reservationRepository.GetByIdAsync(id);
            if (reservation == null)
                throw new Exception("Reservation not found");
                
            reservation.UpdateStatus(ReservationStatus.Cancelled, reason);
            await reservationRepository.UpdateAsync(reservation);
        }

        public async Task ConfirmReservation(string id)
        {
            var reservation = await reservationRepository.GetByIdAsync(id);
            if (reservation == null)
                throw new Exception("Reservation not found");
                
            reservation.UpdateStatus(ReservationStatus.Confirmed);
            await reservationRepository.UpdateAsync(reservation);
        }

        public async Task<string> CreateReservation(CreateReservationDto model)
        {
            Room? room = await roomRepository.GetByIdAsync(model.RoomId);
            if (room == null)
            {
                throw new Exception("Room does not exist");
            }

            var guest = new Guest(model.FirstName, model.LastName, model.Email, model.PhoneNumber);

            int stayDuration = (model.DepartureDate - model.ArrivalDate).Days;
            decimal totalPrice = stayDuration * room.PricePerNight;

            var reservation = new Reservation(
                model.ArrivalDate,
                model.DepartureDate,
                1, 
                totalPrice,
                string.Empty, 
                ReservationStatus.Pending,
                string.Empty, 
                model.RoomId,
                guest.Id, 
                null 
            );

            return await reservationRepository.CreateAsync(reservation);
        }
    }
}
