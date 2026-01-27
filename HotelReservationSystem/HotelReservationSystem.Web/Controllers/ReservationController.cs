using HotelReservationSystem.Application.CQRS;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Application.CQRS.Abstractions;

namespace HotelReservationSystem.Controllers;

public class ReservationController : Controller
{
    private readonly ICQRSMediator mediator;

    public ReservationController(ICQRSMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var query = new GetAllReservationsQuery();
        var reservations = await mediator.SendAsync(query);
        return View(await reservations.ToListAsync());
    }

    [Authorize(Roles = "Recepcjonista, Kierownik")]
    public async Task<IActionResult> List()
    {
        var query = new GetAllReservationsQuery();
        var reservations = await mediator.SendAsync(query);
        return View(await reservations.ToListAsync());
    }

    [Authorize(Roles = "Recepcjonista, Kierownik")]
    public async Task<IActionResult> ReceptionPanel()
    {
        return View();
    }

    public async Task<IActionResult> MyReservations()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var userEmail = User.Identity.Name;
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "Account");
        }

        var query = new GetReservationsByGuestEmailQuery(userEmail);
        var reservations = await mediator.SendAsync(query);

        return View(await reservations.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // TODO: Create GetAvailableRoomsQuery in CQRS structure
        // var roomsQuery = new GetAvailableRoomsQuery(DateTime.Today, DateTime.Today.AddDays(7));
        // var rooms = await mediator.SendAsync(roomsQuery);
        ViewBag.Rooms = new List<SelectListItem>(); // Temporary empty list
        /* (await rooms.ToListAsync()).Select(r => new SelectListItem
        {
            Value = r.Id.ToString(),
            Text = $"{r.Number} ({r.Type}) - {r.PricePerNight} zł"
        }).ToList(); */

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ReservationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // TODO: Create GetAvailableRoomsQuery in CQRS structure
            // var roomsQuery = new GetAvailableRoomsQuery(DateTime.Today, DateTime.Today.AddDays(7));
            // var rooms = await mediator.SendAsync(roomsQuery);
            ViewBag.Rooms = new List<SelectListItem>(); // Temporary empty list
            /*
            ViewBag.Rooms = (await rooms.ToListAsync()).Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = $"{r.Number} ({r.Type}) - {r.PricePerNight} zł"
            }).ToList();
            */

            return View(model);
        }

        var command = new CreateReservationCommand(
            model.ArrivalDate,
            model.DepartureDate,
            model.RoomId,
            model.GuestFirstName,
            model.GuestLastName,
            model.GuestEmail,
            model.GuestPhoneNumber);

        var reservationId = await mediator.SendAsync(command);

        return RedirectToAction("Pay", "Payment", new { reservationId });
    }

    [HttpPost]
    public async Task<IActionResult> MarkPaid([FromBody] MarkPaidDto dto)
    {
        var command = new MarkReservationAsPaidCommand(dto.ReservationId, dto.PaymentIntentId);
        await mediator.SendAsync(command);
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = "Recepcjonista, Kierownik")]
    public async Task<IActionResult> Confirm(string id)
    {
        var command = new ConfirmReservationCommand(id);
        await mediator.SendAsync(command);
        return RedirectToAction("List");
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(string id, string reason)
    {
        var command = new CancelReservationCommand(id, reason);
        await mediator.SendAsync(command);
        return RedirectToAction(nameof(List));
    }

    [Authorize(Roles = "Recepcjonista, Kierownik")]
    [HttpPost]
    public async Task<IActionResult> ToggleRoomAvailability(int id)
    {
        // TODO: Create ToggleRoomAvailabilityCommand in CQRS structure
        // var toggleCommand = new ToggleRoomAvailabilityCommand(id);
        // await mediator.SendAsync(toggleCommand);
        return RedirectToAction(nameof(List));
    }
}