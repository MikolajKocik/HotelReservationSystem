using System.Text.Json;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Core.Domain.Enums;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelReservationSystem.Web.ViewComponents;

public sealed class ReservationModalViewComponent : ViewComponent
{
    private readonly ICQRSMediator mediator;

    public ReservationModalViewComponent(ICQRSMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync(string roomType = "Single")
    {
        var query = new GetAvailableRoomsSelectListQuery(
            DateTime.Today,
            DateTime.Today.AddDays(7)
        );

        List<RoomSelectDto> rooms = await this.mediator.SendAsync(query)
            ?? new List<RoomSelectDto>();

        RoomType requestedType = RoomType.Single;
        if (!string.IsNullOrEmpty(roomType) &&
            Enum.TryParse<RoomType>(roomType, ignoreCase: true, out var parsed))
        {
            requestedType = parsed;
        }

        rooms = rooms.Where(r => r.Type == requestedType).ToList();

        var selectItems = rooms.Select(r => new SelectListItem
        {
            Value = r.Id.ToString(),
            Text = $"{r.Number} ({r.Type}) - {r.PricePerNight} {r.Currency}"
        }).ToList();

        var roomPrices = rooms.ToDictionary(r => r.Id, r => r.PricePerNight);

        var modalModel = new HotelReservationSystem.Web.ViewModels.Reservation.ReservationModalModel
        {
            Reservation = new ReservationViewModel
            {
                FormType = (roomType ?? "Single").ToLower()
            },
            Rooms = selectItems,
            RoomPrices = roomPrices
        };

        return View(modalModel);
    }
}
