using HotelReservationSystem.Application.Dtos.Opinion;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Application.ModelMappings;

public static class OpinionMappings
{
    public static OpinionDto ToDto(this Opinion entity)
    {
        return new OpinionDto
        {
            Id = entity.Id,
            Rating = entity.Rating,
            Comment = entity.Comment,
            CreatedAt = entity.CreatedAt,
            ReservationId = entity.ReservationId,
            GuestId = entity.GuestId,
            GuestFirstName = entity.Guest?.FirstName ?? string.Empty,
            GuestLastName = entity.Guest?.LastName ?? string.Empty,
            RoomId = entity.Reservation?.RoomId ?? 0,
            RoomNumber = entity.Reservation?.Room?.Number ?? string.Empty
        };
    }
}
