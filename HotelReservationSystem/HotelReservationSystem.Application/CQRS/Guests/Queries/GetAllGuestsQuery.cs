using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Guest;

namespace HotelReservationSystem.Application.CQRS.Guests.Queries;

/// <summary>
/// Query to retrieve all guests with their basic information
/// </summary>
public record GetAllGuestsQuery : IQuery<IEnumerable<GuestDto>>;
