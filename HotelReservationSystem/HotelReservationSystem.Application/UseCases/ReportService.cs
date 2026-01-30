using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Application.UseCases;

public sealed class ReportService : IReportService
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
        var reservations = await this.reservationRepository.GetByDateRangeAsync(from, to);

        var availableRooms = await this.roomRepository.GetAvailableRoomsAsync(from, to);
        int availableRoomsCount = await availableRooms.CountAsync();

        return new ReportDto
        {
            TotalReservations = reservations.Count(),
            ConfirmedReservations = reservations.Count(r => r.Status == ReservationStatus.Confirmed),
            CanceledReservations = reservations.Count(r => r.Status == ReservationStatus.Cancelled),
            TotalPayments = reservations.Where(r => r.Status == ReservationStatus.Confirmed).Sum(r => r.TotalPrice),
            AvailableRooms = availableRoomsCount
        };
    }
}
