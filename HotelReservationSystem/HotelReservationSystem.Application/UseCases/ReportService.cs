using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Application.UseCases
{
    public class ReportService : IReportService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IRoomRepository roomRepository;

        public ReportService(IReservationRepository reservationRepository, IRoomRepository roomRepository)
        {
            this.reservationRepository = reservationRepository;
            this.roomRepository = roomRepository;
        }

        public async Task<ReportDto> GenerateReportAsync(DateTime from, DateTime to)
        {
            var reservations = await reservationRepository.GetByDateRangeAsync(from, to);
            var reservationsList = await reservations.ToListAsync();

            var availableRooms = await roomRepository.GetAvailableRoomsAsync(from, to);
            var availableRoomsCount = await availableRooms.CountAsync();

            return new ReportDto
            {
                TotalReservations = reservationsList.Count,
                ConfirmedReservations = reservationsList.Count(r => r.Status == ReservationStatus.Confirmed),
                CanceledReservations = reservationsList.Count(r => r.Status == ReservationStatus.Cancelled),
                TotalPayments = reservationsList.Where(r => r.Status == ReservationStatus.Confirmed).Sum(r => r.TotalPrice),
                AvailableRooms = availableRoomsCount
            };
        }
    }
}