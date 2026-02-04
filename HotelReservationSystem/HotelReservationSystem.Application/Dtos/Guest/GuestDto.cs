using HotelReservationSystem.Application.Dtos.Reservation;

namespace HotelReservationSystem.Application.Dtos.Guest;

public record GuestDto
{
    public string Id { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public ReservationDto[] Reservations { get; init; } = Array.Empty<ReservationDto>();
}