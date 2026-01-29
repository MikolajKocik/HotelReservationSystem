using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Commands;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Web.Services;
using HotelReservationSystem.Web.Utils.ModelMappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Web.ViewModels.Room;

namespace HotelReservationSystem.Controllers;

[Authorize(Policy = "RequireManager")]
public sealed class RoomController : Controller
{
    private readonly ICQRSMediator mediator;
    private readonly IFileService fileService;

    public RoomController(ICQRSMediator mediator, IFileService fileService)
    {
        this.mediator = mediator;
        this.fileService = fileService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = new GetAllRoomsQuery();
        IQueryable<RoomDto> rooms = await mediator.SendAsync(query);

        List<RoomViewModel> roomViewModels = rooms.Select(
            RoomMappingHelper.MapToRoomViewModel)
            .ToList();

        return View(roomViewModels);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var query = new GetRoomByIdQuery(id);
        RoomDto? room = await mediator.SendAsync(query);

        if (room == null)
        {
            return NotFound();
        }

        RoomViewModel viewModel = RoomMappingHelper.MapToRoomViewModel(room);
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateRoomViewModel model)
    {
        try
        {
            string? imagePath = null;
            if (model.ImageFile != null)
            {
                imagePath = await fileService.SaveImageFileAsync(model.ImageFile, "rooms");
            }

            var command = new CreateRoomCommand(
                model.Number,
                model.Type,
                model.PricePerNight,
                imagePath
            );

            int roomId = await mediator.SendAsync(command);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var query = new GetRoomByIdQuery(id);
        var room = await mediator.SendAsync(query);

        if (room == null)
        {
            return NotFound();
        }

        EditRoomViewModel viewModel = RoomMappingHelper.MapToEditRoomViewModel(room);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [FromForm] EditRoomViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        try
        {
            string? imagePath = model.ImagePath;
            if (model.ImageFile != null)
            {
                imagePath = await fileService.SaveImageFileAsync(model.ImageFile, "rooms");
            }

            var command = new UpdateRoomCommand(
                model.Id,
                model.PricePerNight,
                model.IsAvailable,
                imagePath
            );

            await mediator.SendAsync(command);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var command = new DeleteRoomCommand(id);
        await mediator.SendAsync(command);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleAvailability(int id)
    {
        var command = new ToggleRoomAvailabilityCommand(id);
        await mediator.SendAsync(command);
        return RedirectToAction(nameof(Index));
    }
}
