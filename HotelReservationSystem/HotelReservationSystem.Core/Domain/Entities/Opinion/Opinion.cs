namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Opinion
{
    private Opinion() 
    {
        this.Id = string.Empty;
        this.ReservationId = string.Empty;
        this.GuestId = string.Empty;
    } 

    public Opinion(double rating, string comment, string reservationId, string guestId)
    {
        ValidateInput(rating, comment);
        
        this.Id = $"opinion_{Guid.NewGuid()}";
        this.Rating = rating;
        this.Comment = comment;
        this.ReservationId = reservationId;
        this.GuestId = guestId;
        this.CreatedAt = DateTime.UtcNow;
    }

    public string Id { get; private set; } = default!;
    public double Rating { get; private set; }
    public string Comment { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }

    public string ReservationId { get; private set; } = default!;
    public Reservation Reservation { get; set; } = default!;

    public string GuestId { get; private set; } = default!;
    public Guest Guest { get; set; } = default!;
}