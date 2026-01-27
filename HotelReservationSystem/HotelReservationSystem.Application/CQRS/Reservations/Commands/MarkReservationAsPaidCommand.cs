using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Reservations.Commands;

/// <summary>
/// Command to mark a reservation as paid
/// </summary>
public record MarkReservationAsPaidCommand(
    string ReservationId,
    string PaymentIntentId
) : ICommand;
