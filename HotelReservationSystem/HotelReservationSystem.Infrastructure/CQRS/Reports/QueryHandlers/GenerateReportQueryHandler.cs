using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Reports.Queries;
using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Infrastructure.CQRS.Reports.QueryHandlers;

/// <summary>
/// Handler for generating reports
/// </summary>
public sealed class GenerateReportQueryHandler : IQueryHandler<GenerateReportQuery, ReportDto>
{
    private readonly IReservationRepository reservationRepository;
    private readonly IRoomRepository roomRepository;

    public GenerateReportQueryHandler(
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository)
    {
        this.reservationRepository = reservationRepository;
        this.roomRepository = roomRepository;
    }

    /// <summary>
    /// Handles the query to generate a report
    /// </summary>
    public async Task<ReportDto> HandleAsync(GenerateReportQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<Reservation> reservations = await this.reservationRepository.GetByDateRangeAsync(query.FromDate, query.ToDate);

        int totalReservations = reservations.Count();
        int confirmedReservations = reservations.Count(r => r.Status == ReservationStatus.Confirmed);
        int canceledReservations = reservations.Count(r => r.Status == ReservationStatus.Cancelled);
        decimal totalPayments = reservations.Where(r => r.Status == ReservationStatus.Confirmed).Sum(r => r.TotalPrice);

        IEnumerable<Room> availableRooms = await this.roomRepository.GetAvailableRoomsAsync(query.FromDate, query.ToDate);
        int availableRoomsCount = availableRooms.Count();

        return new ReportDto
        {
            TotalReservations = totalReservations,
            ConfirmedReservations = confirmedReservations,
            CanceledReservations = canceledReservations,
            TotalPayments = totalPayments,
            AvailableRooms = availableRoomsCount
        };
    }
}