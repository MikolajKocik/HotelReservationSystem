using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Web.ViewModels;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Controllers;

public sealed class HomeController : Controller
{
    private readonly IRoomRepository roomRepository;

    public HomeController(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IQueryable<Room> rooms = await roomRepository.GetAvailableRoomsAsync(DateTime.Today, DateTime.Today.AddDays(1));
        return View(rooms);
    }

    [HttpGet]
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
