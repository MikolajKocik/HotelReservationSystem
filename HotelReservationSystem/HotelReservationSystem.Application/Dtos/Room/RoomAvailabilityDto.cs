namespace HotelReservationSystem.Application.Dtos.Room;

public record RoomAvailabilityDto
{
    public int RoomId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
}