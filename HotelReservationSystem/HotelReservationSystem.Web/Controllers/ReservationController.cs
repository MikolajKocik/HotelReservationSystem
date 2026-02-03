using System.Text.Json;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Commands;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Web.Utils.ModelMappings;
using HotelReservationSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Guests.Queries;
using HotelReservationSystem.Application.Dtos.Guest;
using HotelReservationSystem.Core.Domain.Enums;

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
            .Select(ReservationMappingHelper.MapToReservationViewModel)
            .ToList();

        return View(viewModels);
    }

    [HttpGet]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> List()
    {
        var query = new GetAllReservationsQuery();
        IEnumerable<ReservationDto> reservations = await this.mediator.SendAsync(query)
            ?? Enumerable.Empty<ReservationDto>();

        List<ReservationListViewModel> viewModel = reservations
            .Select(r => new ReservationListViewModel
            {
                Id = r.Id,
                ArrivalDate = r.ArrivalDate,
                DepartureDate = r.DepartureDate,
                RoomNumber = r.RoomNumber,
                GuestFullName = $"{r.GuestFirstName} {r.GuestLastName}",
                TotalPrice = r.TotalPrice,
                Status = r.Status.ToString()
            })
            .ToList();

        return View(viewModel);
    }

    [HttpGet]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> ReceptionPanel()
    {
        IEnumerable<GuestDto> result = await this.mediator.SendAsync(new GetAllGuestsQuery())
            ?? Enumerable.Empty<GuestDto>();

        var viewModel = result
            .Select(GuestMappingHelper.MapToGuestViewModel)
            .ToList();

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> MyReservations()
    {
        if (!(User?.Identity?.IsAuthenticated ?? false))
        {
            return RedirectToAction("Login", "Account");
        }

        string? userEmail = User?.Identity?.Name;
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "Account");
        }

        var query = new GetReservationsByGuestEmailQuery(userEmail);
        IEnumerable<ReservationDto> reservations = await this.mediator.SendAsync(query) 
            ?? Enumerable.Empty<ReservationDto>();

        List<ReservationListViewModel> viewModels = reservations
            .Select(r => new ReservationListViewModel
            {
                Id = r.Id,
                ArrivalDate = r.ArrivalDate,
                DepartureDate = r.DepartureDate,
                RoomNumber = r.RoomNumber,
                GuestFullName = $"{r.GuestFirstName} {r.GuestLastName}",
                TotalPrice = r.TotalPrice,
                Status = r.Status.ToString()
            })
            .ToList();

        return View(viewModels);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return Redirect("/Room/Index#createSingleModal");
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableRoomsByDate(string roomType, DateTime? arrivalDate, DateTime? departureDate)
    {
        RoomType? requestedType = null;
        if (!string.IsNullOrEmpty(roomType) && Enum.TryParse<RoomType>(roomType, ignoreCase: true, out var parsed))
        {
            requestedType = parsed;
        }

        var allRoomsQuery = new GetAllRoomsQuery(requestedType?.ToString());
        List<RoomDto> allRooms = (await this.mediator.SendAsync(allRoomsQuery))
            ?.ToList() ?? new List<RoomDto>();

        HashSet<int> availableIds = new();
        bool hasValidDates = arrivalDate.HasValue && departureDate.HasValue && arrivalDate < departureDate;

        if (hasValidDates)
        {
            var availableQuery = new GetAvailableRoomsSelectListQuery(arrivalDate!.Value, departureDate!.Value);
            List<RoomSelectDto> availableRooms = await this.mediator.SendAsync(availableQuery) ?? new List<RoomSelectDto>();
            availableIds = availableRooms.Select(r => r.Id).ToHashSet();
        }

        var result = allRooms
            .Where(r => !requestedType.HasValue || r.Type == requestedType.Value)
            .Select(r =>
            {
                string statusLabel = !hasValidDates
                    ? "[Wybierz daty]"
                    : (r.IsAvailable && availableIds.Contains(r.Id)) ? "[Wolny]" : "[Zajęty]";

                return new
                {
                    id = r.Id,
                    number = r.Number,
                    type = r.Type.ToString(),
                    pricePerNight = r.PricePerNight,
                    currency = r.Currency,
                    statusLabel,
                    text = $"{r.Number} ({r.Type}) - {r.PricePerNight} {r.Currency}"
                };
            });

        return Json(new { rooms = result });
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
        try
        {    
            if (!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(new { error = "Nieprawidłowe dane formularza", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var command = new CreateReservationCommand(
                model.ArrivalDate,
                model.DepartureDate,
                model.RoomId,
                model.GuestFirstName,
                model.GuestLastName,
                model.GuestEmail,
                model.GuestPhoneNumber,
                model.DiscountCode,
                model.AdditionalRequests,
                model.AcceptPrivacy
            );

            string result = await this.mediator.SendAsync(command);
            string redirectUrl = Url.Action("Pay", "Payment", new { reservationId = result }) ?? "/";
            return Json(new { redirectUrl });
        }
        catch (Exception ex)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Json(new { error = $"Błąd podczas tworzenia rezerwacji: {ex.Message}" });
        }
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

    private async Task PopulateAvailableRoomsSelectList(int? roomId = null)
    {
        var query = new GetAvailableRoomsSelectListQuery(
            DateTime.Today,
            DateTime.Today.AddDays(7)
        );

        List<RoomSelectDto> rooms = await this.mediator.SendAsync(query) 
            ?? new List<RoomSelectDto>();

        ViewBag.Rooms = rooms.Select(r => new SelectListItem
        {
            Value = r.Id.ToString(),
            Text = $"{r.Number} ({r.Type}) - {r.PricePerNight} {r.Currency}"
        }).ToList();
        ViewBag.RoomsJson = JsonSerializer.Serialize(rooms);
        ViewBag.SelectedRoomId = roomId;
    }
}