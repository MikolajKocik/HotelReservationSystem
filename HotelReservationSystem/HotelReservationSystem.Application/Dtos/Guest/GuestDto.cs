namespace HotelReservationSystem.Application.Dtos.Guest;

public class GuestDto
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public int ReservationsCount { get; set; }
}