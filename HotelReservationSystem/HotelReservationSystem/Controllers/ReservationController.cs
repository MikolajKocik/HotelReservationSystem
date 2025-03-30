using HotelReservationSystem.Models.ViewModels;
using HotelReservationSystem.Repositories.Interfaces;
using HotelReservationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IReservationRepository _reservationRepository;

        public ReservationController(IReservationService reservationService,
            IReservationRepository reservationRepository)
        {
            _reservationService = reservationService;
            _reservationRepository = reservationRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult List()
        {
            var reservations = _reservationRepository.GetAll(); 
            if(reservations == null)
            {
                return NotFound();
            }
            return View(reservations);
        }


        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _reservationService.CreateReservation(model);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            await _reservationService.ConfirmReservation(id);
            return RedirectToAction(nameof(List));

        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, string reason)
        {
            await _reservationService.CancelReservation(id, reason);
            return RedirectToAction(nameof(List));

        }
    }
}
