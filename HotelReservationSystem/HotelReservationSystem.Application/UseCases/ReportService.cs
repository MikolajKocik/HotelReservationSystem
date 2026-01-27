using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.UseCases
{
    public class ReportService : IReportService
    {
        private readonly IReservationRepository reservationRepository;

        public ReportService(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        public async Task<ReportDto> GenerateReportAsync(DateTime from, DateTime to)
        {
            var reservations = await reservationRepository.GetByDateRangeAsync(from, to);

            return new ReportDto
            {
                TotalReservations = reservations.Count(),
                Confirmed = reservations.Count(r => r.Status == ReservationStatus.Confirmed),
                TotalIncome = reservations.Where(r => r.Payment != null).Sum(r => r.Payment.Amount)
            };
        }
    }
}