using HotelReservationSystem.Web.ViewModels.Room;

namespace HotelReservationSystem.Web.ViewModels.Reservation;

public sealed record ReservationListPageViewModel
{
    public List<ReservationListViewModel> Reservations { get; init; } = new();
    public List<RoomSelectViewModel> Rooms { get; init; } = new();
}
