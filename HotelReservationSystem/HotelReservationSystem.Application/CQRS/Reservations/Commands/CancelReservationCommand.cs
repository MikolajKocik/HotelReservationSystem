using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Reservations.Commands;

/// <summary>
/// Command to cancel a reservation
/// </summary>
public record CancelReservationCommand(
    string Id,
    string Reason
) : ICommand;
