using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Guests.Commands;

/// <summary>
/// Command to update an existing guest
/// </summary>
public record UpdateGuestCommand(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber
) : ICommand;
