namespace HotelReservationSystem.Application.Dtos.Opinion;

public class OpinionDto
{
    public string Id { get; set; } = default!;
    public double Rating { get; set; }
    public string Comment { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    // Related entities
    public string ReservationId { get; set; } = default!;
    public string GuestId { get; set; } = default!;
    public string GuestFirstName { get; set; } = default!;
    public string GuestLastName { get; set; } = default!;
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = default!;
}