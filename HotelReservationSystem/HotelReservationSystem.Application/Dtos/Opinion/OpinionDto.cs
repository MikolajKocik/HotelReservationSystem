namespace HotelReservationSystem.Application.Dtos.Opinion;

public record OpinionDto
{
    public string Id { get; init; } = string.Empty;
    public double Rating { get; init; }
    public string Comment { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }

    public string ReservationId { get; init; } = string.Empty;
    public string GuestId { get; init; } = string.Empty;
    public string GuestFirstName { get; init; } = string.Empty;
    public string GuestLastName { get; init; } = string.Empty;
    public int RoomId { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
}