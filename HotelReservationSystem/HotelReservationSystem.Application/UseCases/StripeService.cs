using HotelReservationSystem.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace HotelReservationSystem.Application.UseCases
{
    public class StripeService : IStripeService
    {
        private readonly string stripeApiKey;
        public StripeService(IConfiguration configuration)
        {
            stripeApiKey = Environment.GetEnvironmentVariable("STRIPE_API_KEY") ?? configuration["Stripe:SecretKey"]!; 
            if (string.IsNullOrEmpty(stripeApiKey))
            {
                throw new Exception("Klucz API Stripe nie został znaleziony w zmiennych środowiskowych ani konfiguracji.");
            }
            StripeConfiguration.ApiKey = stripeApiKey;
        }

        public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "pln")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)amount * 100, 
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
