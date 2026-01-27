using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Guest;

namespace HotelReservationSystem.Application.CQRS.Guests.Queries;

/// <summary>
/// Query to retrieve a guest by their email address
/// </summary>
public record GetGuestByEmailQuery(string Email) : IQuery<GuestDto?>;
