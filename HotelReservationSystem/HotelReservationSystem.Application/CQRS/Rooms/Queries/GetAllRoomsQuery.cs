using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Room;

namespace HotelReservationSystem.Application.CQRS.Rooms.Queries;

/// <summary>
/// Query to get all rooms
/// </summary>
public record GetAllRoomsQuery(
    string? RoomType = null,
    DateTime? ArrivalDate = null,
    DateTime? DepartureDate = null,
    int? Guests = null,
    string? SearchPhrase = null
) : IQuery<IQueryable<RoomDto>>;