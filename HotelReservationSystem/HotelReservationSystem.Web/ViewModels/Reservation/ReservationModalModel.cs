using Microsoft.AspNetCore.Mvc.Rendering;
using HotelReservationSystem.Web.ViewModels;

namespace HotelReservationSystem.Web.ViewModels.Reservation;

public sealed class ReservationModalModel
{
    public ReservationModalModel() { }

    public ReservationViewModel Reservation { get; init; } = new();
    public List<SelectListItem> Rooms { get; init; } = new();
}
