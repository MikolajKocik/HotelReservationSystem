using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Web.ViewModels;

namespace HotelReservationSystem.Web.Utils.ModelMappings;

public static class ReportMappings
{
    public static ReportViewModel ToViewModel(this ReportDto dto)
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
