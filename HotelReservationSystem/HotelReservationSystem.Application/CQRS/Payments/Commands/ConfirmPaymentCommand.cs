using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Payments.Commands;

/// <summary>
/// Command to confirm a payment
/// </summary>
public record ConfirmPaymentCommand(
    string ReservationId,
    string PaymentIntentId
) : ICommand;