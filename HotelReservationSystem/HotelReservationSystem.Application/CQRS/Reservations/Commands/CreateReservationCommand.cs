using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Reservations.Commands;

/// <summary>
/// Command to create a new reservation
/// </summary>
public record CreateReservationCommand(
    DateTime ArrivalDate,
    DateTime DepartureDate,
    int RoomId,
    string GuestFirstName,
    string GuestLastName,
    string GuestEmail,
    string GuestPhoneNumber,
    string? DiscountCode,
    string? AdditionalRequests,
    bool AcceptPrivacy
) : ICommand<string>;
