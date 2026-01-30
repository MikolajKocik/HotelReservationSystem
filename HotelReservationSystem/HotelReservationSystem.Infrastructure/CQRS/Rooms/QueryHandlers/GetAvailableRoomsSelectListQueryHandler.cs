using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.QueryHandlers;

/// <summary>
/// Handler for retrieving available rooms for reservation creation
/// </summary>
public sealed class GetAvailableRoomsSelectListQueryHandler : IQueryHandler<GetAvailableRoomsSelectListQuery, List<RoomSelectDto>>
{
    private readonly IRoomRepository roomRepository;

    public GetAvailableRoomsSelectListQueryHandler(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    /// <summary>
    /// Handles the query to get available rooms for reservation creation
    /// </summary>
    public async Task<List<RoomSelectDto>> HandleAsync(GetAvailableRoomsSelectListQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Room> rooms = await this.roomRepository.GetAvailableRoomsAsync(query.FromDate, query.ToDate);

        return rooms.Select(r => new RoomSelectDto
        {
            Id = r.Id,
            Number = r.Number,
            Type = r.Type,
            PricePerNight = r.PricePerNight,
            Currency = r.Currency
        }).ToList();
    }
}