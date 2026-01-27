namespace HotelReservationSystem.Application.Dtos.Opinion;

public class CreateOpinionDto
{
    public double Rating { get; set; }
    public string Comment { get; set; } = default!;
    public string ReservationId { get; set; } = default!;
    public string GuestId { get; set; } = default!;
}