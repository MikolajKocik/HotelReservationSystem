using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Web.ViewModels.Room;

public record RoomViewModel
{
    public int Id { get; init; }
    public string Number { get; init; } = string.Empty;
    public RoomType Type { get; init; }
    public decimal PricePerNight { get; init; }
    public bool IsAvailable { get; init; }
    public string? ImagePath { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Currency { get; init; } = string.Empty;
}
