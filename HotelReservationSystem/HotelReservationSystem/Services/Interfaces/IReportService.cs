using HotelReservationSystem.Models.Dtos;

namespace HotelReservationSystem.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> GenerateReportAsync(DateTime from, DateTime to);
    }

}
