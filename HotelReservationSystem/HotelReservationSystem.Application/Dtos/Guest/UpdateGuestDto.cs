namespace HotelReservationSystem.Application.Dtos.Guest;

public class UpdateGuestDto
{
    public string GuestId { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}