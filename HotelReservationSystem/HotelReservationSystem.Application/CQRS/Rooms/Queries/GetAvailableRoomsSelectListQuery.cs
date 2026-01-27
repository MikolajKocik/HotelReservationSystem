using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Room;

namespace HotelReservationSystem.Application.CQRS.Rooms.Queries;

/// <summary>
/// Query to retrieve available rooms for reservation creation
/// </summary>
public record GetAvailableRoomsSelectListQuery(
    DateTime FromDate,
    DateTime ToDate
) : IQuery<List<RoomSelectDto>>;