using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Enums;
using System;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.QueryHandlers;

/// <summary>
/// Handler for retrieving all rooms
/// </summary>
public class GetAllRoomsQueryHandler : IQueryHandler<GetAllRoomsQuery, IQueryable<RoomDto>>
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
            roomsQuery = await roomRepository.GetAvailableRoomsAsync(query.ArrivalDate.Value, query.DepartureDate.Value);
        }
        else
        {
            roomsQuery = await roomRepository.GetAllAsync();
        }

        if (!string.IsNullOrEmpty(query.RoomType) && Enum.TryParse<RoomType>(query.RoomType, out var parsedType))
        {
            roomsQuery = roomsQuery.Where(r => r.Type == parsedType);
        }

        if (!string.IsNullOrEmpty(query.SearchPhrase))
        {
            roomsQuery = roomsQuery.Where(r => r.Number.Contains(query.SearchPhrase));
        }

        return roomsQuery.Select(r => new RoomDto
        {
            Id = r.Id,
            Number = r.Number,
            Type = r.Type,
            PricePerNight = r.PricePerNight,
            IsAvailable = r.IsAvailable,
            ImagePath = r.ImagePath,
            Currency = r.Currency
        });
    }
}