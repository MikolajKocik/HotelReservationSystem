using HotelReservationSystem.Application.CQRS.Rooms.Queries;
using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Application.CQRS.Abstractions.Queries;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.Queries;

public sealed class GetRoomsByDateQueryHandler : IQueryHandler<GetRoomsByDateQuery, List<RoomAvailabilityDto>>
{
    private readonly IRoomRepository roomRepository;

    public GetRoomsByDateQueryHandler(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    public async Task<List<RoomAvailabilityDto>> HandleAsync(GetRoomsByDateQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Room> allRoomsQuery = await roomRepository.GetRoomsByTypeAsync(query.RoomType, cancellationToken);
        List<Room> allRooms = await allRoomsQuery.ToListAsync(cancellationToken);

        HashSet<int> availableIds = new();
        bool hasValidDates = query.ArrivalDate.HasValue &&
                           query.DepartureDate.HasValue &&
                           query.ArrivalDate < query.DepartureDate;

        if (hasValidDates)
        {
            IQueryable<Room>? availableRoomsQuery = await roomRepository.GetAvailableRoomsAsync(
                query.ArrivalDate!.Value,
                query.DepartureDate!.Value,
                cancellationToken);
            
            List<int> idsList = await availableRoomsQuery
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            availableIds = idsList.ToHashSet();
        }

        return allRooms.Select(r =>
        {
            string statusLabel = !hasValidDates
                ? "[Wybierz daty]"
                : (r.IsAvailable && availableIds.Contains(r.Id)) ? "[Wolny]" : "[Zajęty]";

            return new RoomAvailabilityDto
            {
                Id = r.Id,
                Number = r.Number,
                Type = r.Type.ToString(),
                PricePerNight = r.PricePerNight,
                Currency = r.Currency,
                StatusLabel = statusLabel,
                Text = $"{r.Number} ({r.Type}) - {r.PricePerNight} {r.Currency}"
            };
        }).ToList();
    }
}
