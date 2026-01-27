using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Room;

namespace HotelReservationSystem.Application.CQRS.Rooms.Queries;

/// <summary>
/// Query to get a room by its ID
/// </summary>
public record GetRoomByIdQuery(
    int Id
) : IQuery<RoomDto?>;