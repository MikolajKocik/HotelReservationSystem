using HotelReservationSystem.Application.Dtos.Report;

namespace HotelReservationSystem.Application.Interfaces;

public interface IExportService
{
    Task<byte[]> GenerateCsvAsync(ReportDto dto);
    Task<byte[]> GenerateExcelAsync(ReportDto dto);
    Task<byte[]> GeneratePdfAsync(ReportDto dto);
}
