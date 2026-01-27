using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Payment;

public class UpdatePaymentStatusDto
{
    public int PaymentId { get; set; }
    public PaymentStatus Status { get; set; }
}