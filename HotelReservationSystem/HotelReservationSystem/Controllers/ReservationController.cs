using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.Dtos;
using HotelReservationSystem.Models.ViewModels;
using HotelReservationSystem.Repositories.Interfaces;
using HotelReservationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelReservationSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;

        public ReservationController(IReservationService reservationService,
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository)
        {
            _reservationService = reservationService;
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
        }

        [Authorize(Roles = "Recepcjonista, Kierownik")]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var guests = await _reservationRepository.GetGuests();

            return View("ReceptionPanel", guests);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var reservations = await _reservationRepository.GetAll();
            var rooms = await _roomRepository.GetAll(); 
            if(reservations == null)
            {
                return NotFound();
            }

            ViewBag.Rooms = rooms;

            return View(reservations);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var rooms = await _roomRepository.GetAvailableRoomsAsync(DateTime.Today, DateTime.Today.AddDays(7));
            ViewBag.Rooms = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = $"{r.Number} ({r.Type}) - {r.PricePerNight} zł"
            }).ToList();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {

                var rooms = await _roomRepository.GetAvailableRoomsAsync(DateTime.Today, DateTime.Today.AddDays(7));
                ViewBag.Rooms = rooms.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"{r.Number} ({r.Type}) - {r.PricePerNight} zł"
                }).ToList();

                return View(model);
            }

            var reservationId = await _reservationService.CreateReservation(model);

            return RedirectToAction("Pay", "Payment", new { reservationId });
        }

        [HttpPost]
        public async Task<IActionResult> MarkPaid([FromBody] MarkPaidDto dto)
        {
            var reservation = await _reservationRepository.GetById(dto.ReservationId);

            if (reservation == null) return NotFound();

            reservation.Status = "Opłacona";

            reservation.Payment = new Payment
            {
                Method = "Stripe",
                Status = "Opłacona",
                Amount = 200,
                StripePaymentIntentId = dto.PaymentIntentId,
            };

            await _reservationRepository.Update(reservation);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Recepcjonista, Kierownik")]
        public async Task<IActionResult> Confirm(int id)
        {
            await _reservationService.ConfirmReservation(id);
            return RedirectToAction("List");

        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, string reason)
        {
            await _reservationService.CancelReservation(id, reason);
            return RedirectToAction(nameof(List));

        }

        [Authorize(Roles = "Recepcjonista, Kierownik")]
        [HttpPost]
        public async Task<IActionResult> ToggleRoomAvailability(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return NotFound();

            room.IsAvailable = !room.IsAvailable;
            await _roomRepository.UpdateAsync(room);

            return RedirectToAction(nameof(List));
        }

    }
}
