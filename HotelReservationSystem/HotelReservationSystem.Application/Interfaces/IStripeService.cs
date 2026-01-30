namespace HotelReservationSystem.Application.Interfaces;

public interface IStripeService
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "pln");
    Task<string> CreateCheckoutSessionAsync(string reservationId, decimal amount, string currency = "pln", string successUrl = "", string cancelUrl = "");
    Task RefundPaymentAsync(string paymentIntentId);
}
