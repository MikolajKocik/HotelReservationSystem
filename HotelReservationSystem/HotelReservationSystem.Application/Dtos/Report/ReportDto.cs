namespace HotelReservationSystem.Application.Dtos.Report;

public record ReportDto
{
    public int TotalReservations { get; init; }
    public int ConfirmedReservations { get; init; }
    public int CanceledReservations { get; init; }
    public decimal TotalPayments { get; init; }
    public int AvailableRooms { get; init; }
}