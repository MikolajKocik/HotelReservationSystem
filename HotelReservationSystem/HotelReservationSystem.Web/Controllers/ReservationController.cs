using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Commands;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Web.Utils.ModelMappings;
using HotelReservationSystem.Web.ViewModels.Reservation;
using HotelReservationSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Guests.Queries;
using HotelReservationSystem.Application.Dtos.Guest;

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
        IEnumerable<ReservationDto> reservations = await this.mediator.SendAsync(query) 
            ?? Enumerable.Empty<ReservationDto>();

        List<ReservationViewModel> viewModels = reservations
            .Select(r => r.ToViewModel())
            .ToList();

        return View(viewModels);
    }

    [HttpGet]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> List()
    {
        IEnumerable<ReservationDto> reservations = await mediator.SendAsync(new GetAllReservationsQuery()) 
            ?? Enumerable.Empty<ReservationDto>();

        var roomsQuery = new GetAllRoomsQuery(null, null, null, null, null);
        IQueryable<RoomDto> roomsQueryable = await mediator.SendAsync(roomsQuery);
        List<RoomDto> rooms = roomsQueryable?.ToList() ?? new List<RoomDto>();

        var viewModel = new ReservationListPageViewModel
        {
            Reservations = reservations.Select(r => r.ToListViewModel()).ToList(),
            Rooms = rooms.Select(r => r.ToSelectViewModel()).ToList()
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> MyReservations()
    {
        string? email = User?.Identity?.Name;
        if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Account");

        IEnumerable<ReservationDto> reservations = await mediator.SendAsync(new GetReservationsByGuestEmailQuery(email)) 
            ?? Enumerable.Empty<ReservationDto>();

        return View(reservations?
            .Select(r => r.ToListViewModel())
            .ToList() ?? new());
    }

    [HttpGet]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> ReceptionPanel()
    {
        IEnumerable<GuestDto> guests = await mediator.SendAsync(new GetAllGuestsQuery()) 
            ?? Enumerable.Empty<GuestDto>();

        return View(guests
            .Select(g => g.ToViewModel())
            .ToList());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return Redirect("/Room/Index#createSingleModal");
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableRoomsByDate(string roomType, DateTime? arrivalDate, DateTime? departureDate)
    {
        var query = new GetRoomsByDateQuery(roomType, arrivalDate, departureDate);
        var rooms = await mediator.SendAsync(query);
        return Json(new { rooms });
    }

    [HttpGet]
    public async Task<IActionResult> CheckRoomAvailability(int roomId, DateTime? arrivalDate, DateTime? departureDate)
    {
        if (!arrivalDate.HasValue || !departureDate.HasValue || arrivalDate >= departureDate)
        {
            return Json(new { available = false, message = "[Nieprawidłowe daty]" });
        }

        var query = new GetAvailableRoomsSelectListQuery(arrivalDate.Value, departureDate.Value);
        List<RoomSelectDto> rooms = await this.mediator.SendAsync(query) ?? new List<RoomSelectDto>();

        bool isAvailable = rooms.Any(r => r.Id == roomId);
        string message = isAvailable ? "[Wolny]" : "[Zajęty]";

        return Json(new { available = isAvailable, message });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ReservationViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { error = "Validation errors", details = ModelState.Values.SelectMany(v => v.Errors) });

        CreateReservationCommand command = model.ToCreateCommand();
        string reservationId = await this.mediator.SendAsync(command);
        
        return Json(new { redirectUrl = Url.Action("Pay", "Payment", new { reservationId }) });
    }

    [HttpPost]
    public async Task<IActionResult> MarkPaid([FromBody] MarkPaidDto dto)
    {
        var command = new MarkReservationAsPaidCommand(dto.ReservationId, dto.PaymentIntentId);
        await this.mediator.SendAsync(command);
        return Ok();
    }

    [HttpPost]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> Confirm(string id)
    {
        var command = new ConfirmReservationCommand(id);
        await this.mediator.SendAsync(command);
        return RedirectToAction("List");
    }

    [HttpPost]
    [Authorize(Policy = "RequireAnyUser")]
    public async Task<IActionResult> Cancel(string id, string reason)
    {
        var command = new CancelReservationCommand(id, reason);
        await this.mediator.SendAsync(command);
        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> ToggleRoomAvailability(int id)
    {
        var toggleCommand = new ToggleRoomAvailabilityCommand(id);
        await this.mediator.SendAsync(toggleCommand);
        return RedirectToAction(nameof(List));
    }
}