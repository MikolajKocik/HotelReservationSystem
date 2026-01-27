using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Room;

namespace HotelReservationSystem.Application.CQRS.Rooms.Queries;

/// <summary>
/// Query to get available rooms for a specific date range
/// </summary>
public record GetAvailableRoomsQuery(
    DateTime FromDate,
    DateTime ToDate
) : IQuery<IQueryable<RoomDto>>;