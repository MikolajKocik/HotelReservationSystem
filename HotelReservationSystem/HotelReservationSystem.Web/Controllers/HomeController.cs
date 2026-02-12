using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Web.ViewModels;

namespace HotelReservationSystem.Controllers;

public sealed class HomeController : Controller
{

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            Messages = new List<MessageViewModel>()
        };
        return View(model);
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
