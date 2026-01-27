using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Room;

public class RoomDto
{
    public int Id { get; set; }
    public string Number { get; set; } = default!;
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
}