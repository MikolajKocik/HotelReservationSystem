using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Web.ViewModels.Room;

namespace HotelReservationSystem.Web.Utils.ModelMappings;

public static class RoomMappings
{
    public static RoomViewModel ToViewModel(this RoomDto dto)
    {
        return new RoomViewModel
        {
            Id = dto.Id,
            Number = dto.Number,
            Type = dto.Type,
            PricePerNight = dto.PricePerNight,
            IsAvailable = dto.IsAvailable,
            ImagePath = dto.ImagePath,
            CreatedAt = dto.CreatedAt,
            Currency = dto.Currency
        };
    }

    public static EditRoomViewModel ToEditViewModel(this RoomDto dto)
    {
        return new EditRoomViewModel
        {
            Id = dto.Id,
            Number = dto.Number,
            Type = dto.Type,
            PricePerNight = dto.PricePerNight,
            IsAvailable = dto.IsAvailable,
            ImagePath = dto.ImagePath
        };
    }

    public static RoomSelectViewModel ToSelectViewModel(this RoomSelectDto dto)
    {
        return new RoomSelectViewModel
        {
            Id = dto.Id,
            Number = dto.Number,
            Type = dto.Type.ToString(),
            PricePerNight = dto.PricePerNight,
            IsAvailable = true
        };
    }

    public static RoomSelectViewModel ToSelectViewModel(this RoomDto dto)
    {
        return new RoomSelectViewModel
        {
            Id = dto.Id,
            Number = dto.Number,
            Type = dto.Type.ToString(),
            PricePerNight = dto.PricePerNight,
            IsAvailable = dto.IsAvailable
        };
    }
}
