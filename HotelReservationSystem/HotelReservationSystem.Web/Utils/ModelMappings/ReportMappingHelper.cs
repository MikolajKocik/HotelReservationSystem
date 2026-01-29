using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Web.ViewModels;

namespace HotelReservationSystem.Web.Utils.ModelMappings;

public static class ReportMappingHelper
{
    public static ReportViewModel MapToReportViewModel(ReportDto dto)
    {
        return new ReportViewModel
        {
            TotalReservations = dto.TotalReservations,
            ConfirmedReservations = dto.ConfirmedReservations,
            CanceledReservations = dto.CanceledReservations,
            TotalPayments = dto.TotalPayments,
            AvailableRooms = dto.AvailableRooms
        };
    }
}