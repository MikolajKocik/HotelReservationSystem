using HotelReservationSystem.Application.Dtos.Reservation;

namespace HotelReservationSystem.Application.Interfaces
{
    public interface IReservationService
    {
        Task<string> CreateReservation(CreateReservationDto model);
        Task ConfirmReservation(string id);
        Task CancelReservation(string id, string reason);
    }
}
