using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Commands;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Web.Utils;

namespace HotelReservationSystem.Controllers;

public sealed class ReservationController : Controller
{
    private readonly ICQRSMediator mediator;

    public ReservationController(ICQRSMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = new GetAllReservationsQuery();
        IQueryable<ReservationDto> reservations = await mediator.SendAsync(query);

        var viewModels = reservations.Select(
            ReservationMappingHelper.MapToReservationViewModel)
            .ToList();

        return View(viewModels);
    }

    [HttpGet]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> List()
    {
        var query = new GetAllReservationsQuery();
        IQueryable<ReservationDto> reservations = await mediator.SendAsync(query);

        var viewModels = reservations.Select(
            ReservationMappingHelper.MapToReservationViewModel)
            .ToList();

        return View(viewModels);
    }

    [HttpGet]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> ReceptionPanel()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> MyReservations()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        string? userEmail = User.Identity.Name;
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "Account");
        }

        var query = new GetReservationsByGuestEmailQuery(userEmail);
        IQueryable<ReservationDto> reservations = await mediator.SendAsync(query);

        var viewModels = reservations.Select(
            ReservationMappingHelper.MapToReservationViewModel)
            .ToList();

        return View(viewModels);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var query = new GetAvailableRoomsSelectListQuery(
            DateTime.Today,
            DateTime.Today.AddDays(7)
        );

        List<RoomSelectDto> rooms = await mediator.SendAsync(query);

        ViewBag.Rooms = rooms.Select(r => new SelectListItem
        {
            Value = r.Id.ToString(),
            Text = $"{r.Number} ({r.Type}) - {r.PricePerNight} {r.Currency}"
        }).ToList();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ReservationViewModel model)
    {
        try
        {
            var command = new CreateReservationCommand(
                model.ArrivalDate,
                model.DepartureDate,
                model.RoomId,
                model.GuestFirstName,
                model.GuestLastName,
                model.GuestEmail,
                model.GuestPhoneNumber);

            string reservationId = await mediator.SendAsync(command);

            return RedirectToAction("Pay", "Payment", new { reservationId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var query = new GetAvailableRoomsSelectListQuery(
                DateTime.Today,
                DateTime.Today.AddDays(7)
            );

            List<RoomSelectDto> rooms = await mediator.SendAsync(query);

            ViewBag.Rooms = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = $"{r.Number} ({r.Type}) - {r.PricePerNight} {r.Currency}"
            }).ToList();

            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> MarkPaid([FromBody] MarkPaidDto dto)
    {
        var command = new MarkReservationAsPaidCommand(dto.ReservationId, dto.PaymentIntentId);
        await mediator.SendAsync(command);
        return Ok();
    }

    [HttpPost]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> Confirm(string id)
    {
        var command = new ConfirmReservationCommand(id);
        await mediator.SendAsync(command);
        return RedirectToAction("List");
    }

    [HttpPost]
    [Authorize(Policy = "RequireAnyUser")]
    public async Task<IActionResult> Cancel(string id, string reason)
    {
        var command = new CancelReservationCommand(id, reason);
        await mediator.SendAsync(command);
        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> ToggleRoomAvailability(int id)
    {
        var toggleCommand = new ToggleRoomAvailabilityCommand(id);
        await mediator.SendAsync(toggleCommand);
        return RedirectToAction(nameof(List));
    }
}