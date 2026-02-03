using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.QueryHandlers;

/// <summary>
/// Handler for retrieving available rooms for a date range
/// </summary>
public sealed class GetAvailableRoomsQueryHandler : IQueryHandler<GetAvailableRoomsQuery, IQueryable<RoomDto>>
{
    private readonly IRoomRepository roomRepository;

    public GetAvailableRoomsQueryHandler(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    /// <summary>
    /// Handles the query to get available rooms
    /// </summary>
    public async Task<IQueryable<RoomDto>> HandleAsync(GetAvailableRoomsQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Room> rooms = await roomRepository.GetAvailableRoomsAsync(query.FromDate, query.ToDate, cancellationToken);

        return rooms.Select(r => r.ToDto());
    }
}