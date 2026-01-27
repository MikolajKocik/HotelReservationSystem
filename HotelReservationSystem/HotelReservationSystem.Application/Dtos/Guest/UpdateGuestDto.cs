namespace HotelReservationSystem.Application.Dtos.Guest;

public record UpdateGuestDto
{
    public string GuestId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
}