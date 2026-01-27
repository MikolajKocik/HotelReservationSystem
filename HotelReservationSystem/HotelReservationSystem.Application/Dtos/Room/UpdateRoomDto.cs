namespace HotelReservationSystem.Application.Dtos.Room;

public class UpdateRoomDto
{
    public int Id { get; set; }
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImagePath { get; set; }
}