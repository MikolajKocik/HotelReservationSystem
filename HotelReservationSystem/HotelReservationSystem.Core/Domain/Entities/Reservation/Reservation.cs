using HotelReservationSystem.Core.Domain.Enums;
namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Reservation
{
    private Reservation() 
    {
        this.Id = string.Empty;
        this.GuestId = string.Empty;
    }
    
    public Reservation(
        DateTime arrivalDate,
        DateTime departureDate,
        int numberOfGuests,
        decimal totalPrice,
        string additionalRequests,
        ReservationStatus status,
        string reason,
        int roomId,
        string guestId,
        int? paymentId = null
    )
    {
        
        this.Id = $"res_{Guid.NewGuid()}";
        this.ArrivalDate = arrivalDate;
        this.DepartureDate = departureDate;
        this.NumberOfGuests = numberOfGuests;
        this.TotalPrice = totalPrice;
        this.AdditionalRequests = additionalRequests;
        this.Status = status;
        this.Reason = reason;
        this.RoomId = roomId;
        this.GuestId = guestId;
        this.PaymentId = paymentId;
        this.CreatedAt = DateTime.UtcNow;
    }

    public string Id { get; private set; }
    public DateTime ArrivalDate { get; private set; }
    public DateTime DepartureDate { get; private set; }
    public DateTime CreatedAt { get; set; }
    public int NumberOfGuests { get; private set; }
    public decimal TotalPrice { get; private set; }

    public string AdditionalRequests { get; private set; } = default!;

    public ReservationStatus Status { get; private set; }
    public string Reason { get; private set; } = default!;

    public int RoomId { get; private set; }
    public Room Room { get; set; } = default!;

    public string GuestId { get; private set; }
    public Guest Guest { get; set; } = default!;

    public int? PaymentId { get; private set; } 
    public Payment? Payment { get; set; }
}
