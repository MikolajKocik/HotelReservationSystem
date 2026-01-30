using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.QueryHandlers;

/// <summary>
/// Handler for retrieving a room by ID
/// </summary>
public sealed class GetRoomByIdQueryHandler : IQueryHandler<GetRoomByIdQuery, RoomDto?>
{
    private readonly IRoomRepository roomRepository;

    public GetRoomByIdQueryHandler(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    /// <summary>
    /// Handles the query to get a room by ID
    /// </summary>
    public async Task<RoomDto?> HandleAsync(GetRoomByIdQuery query, CancellationToken cancellationToken = default)
    {
        Room? room = await this.roomRepository.GetByIdAsync(query.Id);

        if (room == null)
            return null;

        return new RoomDto
        {
            Id = room.Id,
            Number = room.Number,
            Type = room.Type,
            PricePerNight = room.PricePerNight,
            IsAvailable = room.IsAvailable,
            ImagePath = room.ImagePath,
            CreatedAt = room.CreatedAt
        };
    }
}