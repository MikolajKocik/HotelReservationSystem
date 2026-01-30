namespace HotelReservationSystem.Web.ViewModels;

public record ReservationDetailsViewModel
{
    public string Id { get; init; } = string.Empty;
    public DateTime ArrivalDate { get; init; }
    public DateTime DepartureDate { get; init; }
    public int NumberOfGuests { get; init; }
    public decimal TotalPrice { get; init; }
    public string AdditionalRequests { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
    public string RoomType { get; init; } = string.Empty;
    public decimal RoomPricePerNight { get; init; }
    public string GuestFullName { get; init; } = string.Empty;
    public string GuestEmail { get; init; } = string.Empty;
    public string? PaymentStatus { get; init; }
}