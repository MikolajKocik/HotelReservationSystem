using HotelReservationSystem.Models.ViewModels;
using HotelReservationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _reservationService.CreateReservation(model);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            await _reservationService.ConfirmReservation(id);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, string reason)
        {
            await _reservationService.CancelReservation(id, reason);
            return RedirectToAction("List");
        }
    }
}
