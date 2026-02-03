namespace HotelReservationSystem.Application.Interfaces;

public interface IStripeService
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "pln");
    Task<string> CreateCheckoutSessionAsync(string reservationId, decimal amount, string currency = "pln", string successUrl = "", string cancelUrl = "");
    Task<string?> GetCheckoutSessionPaymentIntentIdAsync(string sessionId);
    Task RefundPaymentAsync(string paymentIntentId);
}
