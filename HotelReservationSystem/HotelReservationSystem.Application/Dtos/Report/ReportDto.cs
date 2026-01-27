namespace HotelReservationSystem.Application.Dtos.Report;

public class ReportDto
{
    public int TotalReservations { get; set; }
    public int Confirmed { get; set; }
    public decimal TotalIncome { get; set; }
}