using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Application.CQRS.Abstractions.Queries;

namespace HotelReservationSystem.Application.CQRS.Rooms.Queries;

public sealed record GetRoomsByDateQuery(
    string? RoomType,
    DateTime? ArrivalDate,
    DateTime? DepartureDate
) : IQuery<List<RoomAvailabilityDto>>;
