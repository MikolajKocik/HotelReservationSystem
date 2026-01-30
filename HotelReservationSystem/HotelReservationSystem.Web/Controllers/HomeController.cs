using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Web.ViewModels;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using System.ComponentModel.Design;

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
        return View();
    }

    [HttpGet]
    public IActionResult Gallery()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
