using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Payments.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace HotelReservationSystem.Controllers;

[ApiController]
[Route("stripe")]
public sealed class StripeController : ControllerBase
{
    private readonly ICQRSMediator mediator;
    private readonly IConfiguration configuration;

    public StripeController(ICQRSMediator mediator, IConfiguration configuration)
    {
        this.mediator = mediator;
        this.configuration = configuration;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        
        string? signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        string? webhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET") 
            ?? configuration["Stripe:WebhookSecret"];

        Event stripeEvent;
        try
        {
            if (!string.IsNullOrEmpty(webhookSecret))
            {
                stripeEvent = EventUtility.ConstructEvent(json, signature, webhookSecret);
            }
            else
            {
                stripeEvent = EventUtility.ParseEvent(json);
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Webhook error: {ex.Message}");
        }

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":

                Session? session = stripeEvent.Data.Object as Session;

                if (session != null)
                {
                    string reservationId = session.Metadata != null && session.Metadata.ContainsKey("reservationId") 
                        ? session.Metadata["reservationId"] 
                        : string.Empty;

                    string? paymentIntentId = session.PaymentIntentId ?? string.Empty;
                    
                    if (!string.IsNullOrEmpty(paymentIntentId))
                    {
                        var markCommand = new MarkReservationAsPaidCommand(reservationId, paymentIntentId);
                        await mediator.SendAsync(markCommand);
                    }
                }
                break;

            case "payment_intent.payment_failed":
                PaymentIntent? pi = stripeEvent.Data.Object as PaymentIntent;
                if (pi != null)
                {
                    string paymentIntentId = pi.Id;
                    
                    var refuseCommand = new RefusePaymentCommand(paymentIntentId);
                    await mediator.SendAsync(refuseCommand);
                }
                break;
        }

        return Ok();
    }
}
