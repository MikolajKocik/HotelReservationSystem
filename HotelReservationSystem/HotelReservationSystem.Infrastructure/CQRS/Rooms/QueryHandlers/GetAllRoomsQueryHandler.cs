using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Enums;
using HotelReservationSystem.Application.ModelMappings;
using System;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.QueryHandlers;

/// <summary>
/// Handler for retrieving all rooms
/// </summary>
public sealed class GetAllRoomsQueryHandler : IQueryHandler<GetAllRoomsQuery, IQueryable<RoomDto>>
{
    private readonly IRoomRepository roomRepository;

    public GetAllRoomsQueryHandler(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    /// <summary>
    /// Handles the query to get all rooms
    /// </summary>
    public async Task<IQueryable<RoomDto>> HandleAsync(GetAllRoomsQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Room> roomsQuery;

        if (query.ArrivalDate.HasValue && query.DepartureDate.HasValue)
        {
            roomsQuery = await this.roomRepository.GetAvailableRoomsAsync(query.ArrivalDate.Value, query.DepartureDate.Value, cancellationToken);
        }
        else
        {
            roomsQuery = await this.roomRepository.GetAllAsync(cancellationToken);
        }

        if (!string.IsNullOrEmpty(query.RoomType) && Enum.TryParse<RoomType>(query.RoomType, out var parsedType))
        {
            roomsQuery = roomsQuery.Where(r => r.Type == parsedType);
        }

        if (!string.IsNullOrEmpty(query.SearchPhrase))
        {
            roomsQuery = roomsQuery.Where(r => r.Number.Contains(query.SearchPhrase));
        }

        return roomsQuery.Select(r => r.ToDto());
    }
}