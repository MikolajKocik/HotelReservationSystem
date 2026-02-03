using HotelReservationSystem.Application.Dtos.Guest;
using HotelReservationSystem.Web.ViewModels;

namespace HotelReservationSystem.Web.Utils.ModelMappings;

public static class GuestMappings
{
    public static GuestViewModel ToViewModel(this GuestDto dto)
    {
        return new GuestViewModel
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };
    }
}
