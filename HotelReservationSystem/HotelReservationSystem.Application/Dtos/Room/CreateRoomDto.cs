using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Room;

public record CreateRoomDto
{
    public string Number { get; init; } = string.Empty;
    public RoomType Type { get; init; }
    public decimal PricePerNight { get; init; }
    public string? ImagePath { get; init; }
}