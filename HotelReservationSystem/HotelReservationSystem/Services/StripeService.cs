using HotelReservationSystem.Services.Interfaces;
using Stripe;

namespace HotelReservationSystem.Services
{
    public class StripeService : IStripeService
    {
        public StripeService(IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
        }

        public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "pln")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)amount * 100, // kwota w groszach
                Currency = currency,
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return paymentIntent.ClientSecret;
        }
    }
}
