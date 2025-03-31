using HotelReservationSystem.Services.Interfaces;
using Stripe;

namespace HotelReservationSystem.Services
{
    public class StripeService : IStripeService
    {
        private readonly string _stripeApiKey;
        public StripeService(IConfiguration configuration)
        {
            _stripeApiKey = Environment.GetEnvironmentVariable("STRIPE_API_KEY") ?? configuration["Stripe:SecretKey"]!; 
            if (string.IsNullOrEmpty(_stripeApiKey))
            {
                throw new Exception("Klucz API Stripe nie został znaleziony w zmiennych środowiskowych ani konfiguracji.");
            }
            StripeConfiguration.ApiKey = _stripeApiKey;
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
