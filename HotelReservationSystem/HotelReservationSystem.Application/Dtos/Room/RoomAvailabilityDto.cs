namespace HotelReservationSystem.Application.Dtos.Room;

public class RoomAvailabilityDto
{
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}