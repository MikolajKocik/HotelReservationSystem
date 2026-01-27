using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Reservations.Commands;

/// <summary>
/// Command to confirm a reservation
/// </summary>
public record ConfirmReservationCommand(string Id) : ICommand;
