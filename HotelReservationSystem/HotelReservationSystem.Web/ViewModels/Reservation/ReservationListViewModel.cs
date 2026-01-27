namespace HotelReservationSystem.Web.ViewModels;

public class ReservationListViewModel
{
    public string Id { get; set; } = string.Empty;
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string GuestFullName { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
}
