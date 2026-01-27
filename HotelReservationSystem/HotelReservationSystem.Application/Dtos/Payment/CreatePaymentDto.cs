namespace HotelReservationSystem.Application.Dtos.Payment;

public class CreatePaymentDto
{
    public string Method { get; set; } = default!;
    public decimal Amount { get; set; }
    public string StripePaymentIntentId { get; set; } = default!;
    public string ReservationId { get; set; } = default!;
}