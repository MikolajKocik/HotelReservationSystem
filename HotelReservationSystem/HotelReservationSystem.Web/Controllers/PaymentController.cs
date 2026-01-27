using HotelReservationSystem.Application.CQRS;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ICQRSMediator mediator;
        private readonly IStripeService stripeService;
        private readonly IConfiguration configuration;

        public PaymentController(
            ICQRSMediator mediator,
            IStripeService stripeService,
            IConfiguration configuration)
        {
            this.mediator = mediator;
            this.stripeService = stripeService;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Pay(string reservationId)
        {
            var query = new GetReservationByIdQuery(reservationId);
            var reservation = await mediator.SendAsync(query);

            if (reservation == null)
                return NotFound("Rezerwacja nie znaleziona.");

            var nights = (reservation.DepartureDate - reservation.ArrivalDate).Days;
            if (nights <= 0)
                nights = 1; 

            var totalAmount = reservation.RoomPricePerNight * nights;

            var clientSecret = await stripeService.CreatePaymentIntentAsync(totalAmount);

            ViewBag.ClientSecret = clientSecret;
            ViewBag.Amount = totalAmount;
            ViewBag.ReservationId = reservationId;
            ViewBag.PublishableKey = Environment.GetEnvironmentVariable("STRIPE_PUBLISHABLE_KEY")
                ?? configuration["Stripe:PublishableKey"];

            if (string.IsNullOrEmpty(ViewBag.PublishableKey))
            {
                return BadRequest("Stripe Publishable Key nie jest skonfigurowany.");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(string reservationId, string paymentMethodId)
        {
            try
            {
                var query = new GetReservationByIdQuery(reservationId);
                var reservation = await mediator.SendAsync(query);

                if (reservation == null)
                    return NotFound("Rezerwacja nie znaleziona.");

                var nights = (reservation.DepartureDate - reservation.ArrivalDate).Days;
                if (nights <= 0)
                    nights = 1;

                var totalAmount = reservation.RoomPricePerNight * nights;

                var paymentIntentId = await stripeService.CreatePaymentIntentAsync(totalAmount);
                
                // TODO: Create ConfirmPaymentCommand in CQRS structure
                // var confirmCommand = new ConfirmPaymentCommand(reservationId, paymentIntentId);
                // await mediator.SendAsync(confirmCommand);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}