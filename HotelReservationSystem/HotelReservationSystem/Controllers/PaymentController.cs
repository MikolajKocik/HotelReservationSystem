using HotelReservationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IStripeService _stripeService;
        private readonly IConfiguration _configuration;

        public PaymentController(IStripeService stripeService, IConfiguration configuration)
        {
            _stripeService = stripeService;
            _configuration = configuration; 
        }

        public async Task<IActionResult> Pay(decimal amount)
        {
            var clientSecret = await _stripeService.CreatePaymentIntentAsync(amount);

            ViewBag.ClientSecret = clientSecret;
            ViewBag.PublishableKey = _configuration["Stripe:PublishableKey"];
            ViewBag.Amount = amount;

            return View();
        }
    }
}
