namespace HotelReservationSystem.Application.Dtos.Opinion;

public record UpdateOpinionDto
{
    public string OpinionId { get; init; } = string.Empty;
    public double Rating { get; init; }
    public string Comment { get; init; } = string.Empty;
}