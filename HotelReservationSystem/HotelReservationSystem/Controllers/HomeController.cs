using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.ViewModels;
using HotelReservationSystem.Repositories.Interfaces;
using HotelReservationSystem.Repositories.EF;

namespace HotelReservationSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRoomRepository _roomRepository;

    public HomeController(ILogger<HomeController> logger, IRoomRepository roomRepository)
    {
        _logger = logger;
        _roomRepository = roomRepository;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await _roomRepository.GetAvailableRoomsAsync(DateTime.Today, DateTime.Today.AddDays(1));
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
