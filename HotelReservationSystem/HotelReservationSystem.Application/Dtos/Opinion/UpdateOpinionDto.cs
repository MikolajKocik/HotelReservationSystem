namespace HotelReservationSystem.Application.Dtos.Opinion;

public class UpdateOpinionDto
{
    public string OpinionId { get; set; } = default!;
    public double Rating { get; set; }
    public string Comment { get; set; } = default!;
}