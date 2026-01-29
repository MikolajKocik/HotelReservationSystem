using HotelReservationSystem.Application.Dtos.Room;
using HotelReservationSystem.Web.ViewModels.Room;

namespace HotelReservationSystem.Web.Utils.ModelMappings
{
    public static class RoomMappingHelper
    {
        public static RoomViewModel MapToRoomViewModel(RoomDto dto)
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

        public static EditRoomViewModel MapToEditRoomViewModel(RoomDto dto)
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
    }
}