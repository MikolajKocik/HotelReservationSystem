using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Application.ModelMappings;

public static class RoomMappings
{
    public static RoomDto ToDto(this Room entity)
    {
        return new RoomDto
        {
            Id = entity.Id,
            Number = entity.Number,
            Type = entity.Type,
            PricePerNight = entity.PricePerNight,
            IsAvailable = entity.IsAvailable,
            ImagePath = entity.ImagePath,
            CreatedAt = entity.CreatedAt,
            Currency = entity.Currency
        };
    }
}
