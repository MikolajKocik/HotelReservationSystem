namespace HotelReservationSystem.Web.ViewModels;

public record ReservationListViewModel
{
    public string Id { get; init; } = string.Empty;
    public DateTime ArrivalDate { get; init; }
    public DateTime DepartureDate { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
    public string GuestFullName { get; init; } = string.Empty;
    public decimal TotalPrice { get; init; }
    public string Status { get; init; } = string.Empty;
}
