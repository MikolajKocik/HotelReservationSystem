using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Web.ViewModels;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IRoomRepository roomRepository;

    public HomeController(ILogger<HomeController> logger, IRoomRepository roomRepository)
    {
        this.logger = logger;
        this.roomRepository = roomRepository;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await roomRepository.GetAvailableRoomsAsync(DateTime.Today, DateTime.Today.AddDays(1));
        return View(rooms);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
