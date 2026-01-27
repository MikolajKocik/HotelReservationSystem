namespace HotelReservationSystem.Web.ViewModels;

public class ReservationDetailsViewModel
{
    public string Id { get; set; } = string.Empty;
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string AdditionalRequests { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public decimal RoomPricePerNight { get; set; }
    public string GuestFullName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public string? PaymentStatus { get; set; }
}