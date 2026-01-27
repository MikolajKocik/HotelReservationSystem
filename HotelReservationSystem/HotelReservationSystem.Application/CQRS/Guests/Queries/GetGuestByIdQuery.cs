using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Guest;

namespace HotelReservationSystem.Application.CQRS.Guests.Queries;

/// <summary>
/// Query to retrieve a specific guest by their unique identifier
/// </summary>
public record GetGuestByIdQuery(string Id) : IQuery<GuestDto?>;
