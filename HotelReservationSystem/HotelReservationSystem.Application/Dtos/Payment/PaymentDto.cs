using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Payment;

public class PaymentDto
{
    public int Id { get; set; }
    public string Method { get; set; } = default!;
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string StripePaymentIntentId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string ReservationId { get; set; } = default!;
}