using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Guests.Commands;

/// <summary>
/// Command to create a new guest
/// </summary>
public record CreateGuestCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber
) : ICommand<string>;
