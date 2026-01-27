namespace HotelReservationSystem.Application.Dtos.Reservation;

public class CreateReservationDto
{
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public int RoomId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}