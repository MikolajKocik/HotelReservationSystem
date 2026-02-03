using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Guests.Queries;
using HotelReservationSystem.Application.Dtos.Guest;
using HotelReservationSystem.Web.Utils.ModelMappings;
using HotelReservationSystem.Web.ViewModels;
using HotelReservationSystem.Web.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HotelReservationSystem.Controllers;

public sealed class GuestController : Controller
{
    private readonly ICQRSMediator mediator;
    private readonly StaffSettings staffSettings;

    public GuestController(ICQRSMediator mediator, IOptions<StaffSettings> staffSettings)
    {
        this.mediator = mediator;
        this.staffSettings = staffSettings.Value;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        IEnumerable<GuestDto> guests = await mediator.SendAsync(new GetAllGuestsQuery())
            ?? Enumerable.Empty<GuestDto>();

        var staffEmailsSet = new HashSet<string>(staffSettings.ExcludedEmails, StringComparer.OrdinalIgnoreCase);

        List<GuestViewModel> viewModels = guests
            .Where(g => !staffEmailsSet.Contains(g.Email))
            .Select(g => g.ToViewModel())
            .ToList();

        return View(viewModels);
    }
}

