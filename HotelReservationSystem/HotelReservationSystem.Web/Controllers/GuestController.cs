using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Guests.Queries;
using HotelReservationSystem.Application.Dtos.Guest;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers;

public sealed class GuestController : Controller
{
    private readonly ICQRSMediator mediator;

    public GuestController(ICQRSMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var query = new GetAllGuestsQuery();
        IQueryable<GuestDto> guests = await mediator.SendAsync(query);

        var viewModels = guests.Select(
            GuestMappingHelper.MapToGuestViewModel).
            ToList();

        return View(viewModels);
    }
}

