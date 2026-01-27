namespace HotelReservationSystem.Application.Dtos.Opinion;

public record CreateOpinionDto
{
    public double Rating { get; init; }
    public string Comment { get; init; } = string.Empty;
    public string ReservationId { get; init; } = string.Empty;
    public string GuestId { get; init; } = string.Empty;
}