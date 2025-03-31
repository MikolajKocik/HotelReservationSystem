namespace HotelReservationSystem.Services.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "pln");
    }
}
