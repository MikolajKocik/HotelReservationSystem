using HotelReservationSystem.Models.ViewModels;

namespace HotelReservationSystem.Services.Interfaces
{
    public interface IReservationService
    {
        Task CreateReservation(ReservationViewModel model);
        Task ConfirmReservation(int id);
        Task CancelReservation(int id, string reason);
    }
}
