﻿using HotelReservationSystem.Repositories.Interfaces;
using HotelReservationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IStripeService _stripeService;
        private readonly IConfiguration _configuration;
        private readonly IReservationRepository _reservationRepository;
        private readonly IGuestRepository _guestRepository;

        public PaymentController(
            IStripeService stripeService,
            IConfiguration configuration,
            IReservationRepository reservationRepository,
            IGuestRepository guestRepository)
        {
            _stripeService = stripeService;
            _configuration = configuration;
            _reservationRepository = reservationRepository;
            _guestRepository = guestRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Pay(int reservationId)
        {
            var reservation = await _reservationRepository.GetById(reservationId);

            if (reservation == null || reservation.Room == null)
                return NotFound("Rezerwacja lub pokój nie znaleziony.");

            var nights = (reservation.DepartureDate - reservation.ArrivalDate).Days;
            if (nights <= 0)
                nights = 1; // awaryjnie – 1 noc minimum

            var totalAmount = reservation.Room.PricePerNight * nights;

            var clientSecret = await _stripeService.CreatePaymentIntentAsync(totalAmount);

            ViewBag.ClientSecret = clientSecret;
            ViewBag.Amount = totalAmount;
            ViewBag.ReservationId = reservationId;
            ViewBag.PublishableKey = Environment.GetEnvironmentVariable("STRIPE_PUBLISHABLE_KEY")
                ?? _configuration["Stripe:PublishableKey"];

            if (string.IsNullOrEmpty(ViewBag.PublishableKey))
                throw new Exception("Publishable key not found.");

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Recepcjonista, Kierownik")]
        public async Task<IActionResult> Transactions()
        {
            var payments = await _guestRepository.GetTransactions();

            return View(payments);
        }

    }
}
