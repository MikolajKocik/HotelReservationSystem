using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

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
        IQueryable<Room> rooms = await roomRepository.GetAllAsync();

        return rooms.Select(r => new RoomDto
        {
            Id = r.Id,
            Number = r.Number,
            Type = r.Type,
            PricePerNight = r.PricePerNight,
            IsAvailable = r.IsAvailable,
            ImagePath = r.ImagePath,
            CreatedAt = r.CreatedAt
        });
    }
}