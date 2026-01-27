using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Web.ViewModels.Room;

public class RoomViewModel
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Currency { get; set; } = string.Empty;
}
