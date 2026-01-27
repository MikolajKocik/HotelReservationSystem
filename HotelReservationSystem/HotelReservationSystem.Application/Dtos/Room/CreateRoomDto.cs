using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Room;

public class CreateRoomDto
{
    public string Number { get; set; } = default!;
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public string? ImagePath { get; set; }
}