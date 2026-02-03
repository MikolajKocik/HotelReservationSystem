using HotelReservationSystem.Application.Dtos.Payment;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Application.ModelMappings;

public static class PaymentMappings
{
    public static PaymentDto ToDto(this Payment entity)
    {
        return new PaymentDto
        {
            Id = entity.Id,
            Method = entity.Method,
            Status = entity.Status,
            Amount = entity.Amount,
            StripePaymentIntentId = entity.StripePaymentIntentId,
            CreatedAt = entity.CreatedAt,
            CompletedAt = entity.CompletedAt,
            ReservationId = entity.ReservationId
        };
    }
}
