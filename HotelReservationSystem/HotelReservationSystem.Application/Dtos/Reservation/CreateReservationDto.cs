namespace HotelReservationSystem.Application.Dtos.Reservation;

public record CreateReservationDto
{
    public DateTime ArrivalDate { get; init; }
    public DateTime DepartureDate { get; init; }
    public int RoomId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string AdditionalRequests { get; init; } = string.Empty;
}