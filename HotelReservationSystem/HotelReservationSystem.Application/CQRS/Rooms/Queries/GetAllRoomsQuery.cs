using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Room;

namespace HotelReservationSystem.Application.CQRS.Rooms.Queries;

/// <summary>
/// Query to get all rooms
/// </summary>
public record GetAllRoomsQuery : IQuery<IQueryable<RoomDto>>;