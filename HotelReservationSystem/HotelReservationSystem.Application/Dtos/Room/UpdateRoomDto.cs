namespace HotelReservationSystem.Application.Dtos.Room;

public record UpdateRoomDto
{
    public int Id { get; init; }
    public decimal PricePerNight { get; init; }
    public bool IsAvailable { get; init; }
    public string? ImagePath { get; init; }
}