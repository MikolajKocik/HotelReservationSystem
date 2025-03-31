using HotelReservationSystem.Models.ViewModels;
using HotelReservationSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = "Kierownik")]
    public class ReportsController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;

        public ReportsController(IReservationRepository reservationRepository, IRoomRepository roomRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var reservations = await _reservationRepository.GetAll();
            var payments = reservations.Select(r => r.Payment?.Amount ?? 0);
            var rooms = await _roomRepository.GetAvailableRoomsAsync(DateTime.Today, DateTime.Today.AddDays(30));

            var model = new ReportViewModel
            {
                TotalReservations = reservations.Count(),
                ConfirmedReservations = reservations.Count(r => r.Status == "Potwierdzona"),
                CanceledReservations = reservations.Count(r => r.Status == "Anulowana"),
                TotalPayments = payments.Sum(),
                AvailableRooms = rooms.Count()
            };

            return View(model);
        }
    }
}
