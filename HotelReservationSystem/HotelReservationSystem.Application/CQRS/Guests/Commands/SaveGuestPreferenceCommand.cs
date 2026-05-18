using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Guests.Commands;

public record SaveGuestPreferenceCommand(
    string email, 
    string category,
    string value
    ) : ICommand;