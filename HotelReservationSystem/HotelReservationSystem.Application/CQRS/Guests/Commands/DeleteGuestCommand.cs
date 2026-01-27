using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Guests.Commands;

/// <summary>
/// Command to delete a guest by their identifier
/// </summary>
public record DeleteGuestCommand(string Id) : ICommand;
