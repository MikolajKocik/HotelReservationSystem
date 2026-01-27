using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Room;

public record RoomSelectDto
{
    public int Id { get; init; }
    public string Number { get; init; } = string.Empty;
    public RoomType Type { get; init; }
    public decimal PricePerNight { get; init; }
    public string Currency { get; init; } = string.Empty;
}