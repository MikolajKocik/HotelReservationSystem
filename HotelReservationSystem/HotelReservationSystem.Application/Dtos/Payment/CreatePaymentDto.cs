namespace HotelReservationSystem.Application.Dtos.Payment;

public record CreatePaymentDto
{
    public string Method { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string StripePaymentIntentId { get; init; } = string.Empty;
    public string ReservationId { get; init; } = string.Empty;
}