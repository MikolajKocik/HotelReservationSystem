using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.CQRS.Reservations.Commands;

/// <summary>
/// Command to update an existing reservation
/// </summary>
public record UpdateReservationCommand(
    string Id,
    DateTime ArrivalDate,
    DateTime DepartureDate,
    ReservationStatus Status,
    string? Reason = null
) : ICommand;
