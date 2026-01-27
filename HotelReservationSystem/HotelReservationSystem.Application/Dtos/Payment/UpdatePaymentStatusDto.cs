using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Payment;

public record UpdatePaymentStatusDto
{
    public int PaymentId { get; init; }
    public PaymentStatus Status { get; init; }
}