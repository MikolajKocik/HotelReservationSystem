using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.QueryHandlers;

/// <summary>
/// Handler for retrieving available rooms for a date range
/// </summary>
public class GetAvailableRoomsQueryHandler : IQueryHandler<GetAvailableRoomsQuery, IQueryable<RoomDto>>
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
        IQueryable<Room> rooms = await roomRepository.GetAvailableRoomsAsync(query.FromDate, query.ToDate);

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