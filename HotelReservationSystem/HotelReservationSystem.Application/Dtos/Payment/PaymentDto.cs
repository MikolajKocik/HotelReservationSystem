using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Payment;

public record PaymentDto
{
    public int Id { get; init; }
    public string Method { get; init; } = string.Empty;
    public PaymentStatus Status { get; init; }
    public decimal Amount { get; init; }
    public string StripePaymentIntentId { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public string ReservationId { get; init; } = string.Empty;
}