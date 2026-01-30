using HotelReservationSystem.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using HotelReservationSystem.Core.Domain.Exceptions;

namespace HotelReservationSystem.Application.UseCases;

public sealed class StripeService : IStripeService
{
    private readonly string stripeApiKey;
    public StripeService(IConfiguration configuration)
    {
        this.stripeApiKey = Environment.GetEnvironmentVariable("STRIPE_API_KEY") ?? configuration["Stripe:SecretKey"]!; 
        if (string.IsNullOrEmpty(this.stripeApiKey))
        {
            throw new ValidationDomainException("Klucz API Stripe nie został znaleziony w zmiennych środowiskowych ani konfiguracji.");
        }
        StripeConfiguration.ApiKey = this.stripeApiKey;
    }

    public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "pln")
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = Convert.ToInt64(Math.Round(amount * 100m)), 
            Currency = currency,
            PaymentMethodTypes = new List<string>
            {
                "card",
            },
        };

        var service = new PaymentIntentService();
        PaymentIntent paymentIntent = await service.CreateAsync(options);

        return paymentIntent.ClientSecret;
    }

    public async Task<string> CreateCheckoutSessionAsync(string reservationId, decimal amount, string currency = "pln", string successUrl = "", string cancelUrl = "")
    {
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            PaymentMethodTypes = new List<string>
            {
                "card",
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = Convert.ToInt64(Math.Round(amount * 100m)), 
                        Currency = currency.ToLower(),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Reservation {reservationId}"
                        }
                    },
                    Quantity = 1
                }
            },
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            Metadata = new Dictionary<string, string>
            {
                { "reservationId", reservationId }
            }
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return session.Url ?? session.Id;
    }

    public async Task RefundPaymentAsync(string paymentIntentId)
    {
        var piService = new PaymentIntentService();

        PaymentIntent pi = await piService.GetAsync(paymentIntentId);

        var chargeService = new ChargeService();
        StripeList<Charge>? charges = await chargeService.ListAsync(new ChargeListOptions 
        { 
            PaymentIntent = pi.Id, 
            Limit = 1 
        });
        Charge? firstCharge = charges.Data.FirstOrDefault();

        if (firstCharge == null)
            throw new NotFoundException("Charge not found for refund.");

        var refundService = new RefundService();
        await refundService.CreateAsync(new RefundCreateOptions 
        { 
            Charge = firstCharge.Id 
        });
    }
}
