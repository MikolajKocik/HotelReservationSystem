using HotelReservationSystem.Application.Dtos.Report;

namespace HotelReservationSystem.Application.Interfaces;

public interface IReportService
{
    Task<ReportDto> GenerateReportAsync(DateTime from, DateTime to);
}
