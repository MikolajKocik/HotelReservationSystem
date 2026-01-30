namespace HotelReservationSystem.Web.ViewModels;

public record ReportViewModel
{
    public int TotalReservations { get; init; }
    public int ConfirmedReservations { get; init; }
    public int CanceledReservations { get; init; }
    public decimal TotalPayments { get; init; }
    public int AvailableRooms { get; init; }
}
