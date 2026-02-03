using HotelReservationSystem.Application.Dtos.Guest;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Application.ModelMappings;

public static class GuestMappings
{
    public static GuestDto ToDto(this Guest entity)
    {
        return new GuestDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email ?? string.Empty,
            PhoneNumber = entity.PhoneNumber ?? string.Empty
        };
    }
}
