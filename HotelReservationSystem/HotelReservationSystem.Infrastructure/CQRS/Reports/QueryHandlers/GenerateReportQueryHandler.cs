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
public class GenerateReportQueryHandler : IQueryHandler<GenerateReportQuery, ReportDto>
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
        IQueryable<Reservation> reservations = await reservationRepository.GetByDateRangeAsync(query.FromDate, query.ToDate);
        List<Reservation> reservationsList = await reservations.ToListAsync();

        int totalReservations = reservationsList.Count;
        int confirmedReservations = reservationsList.Count(r => r.Status == ReservationStatus.Confirmed);
        int canceledReservations = reservationsList.Count(r => r.Status == ReservationStatus.Cancelled);
        decimal totalPayments = reservationsList.Where(r => r.Status == ReservationStatus.Confirmed).Sum(r => r.TotalPrice);

        IQueryable<Room> availableRooms = await roomRepository.GetAvailableRoomsAsync(query.FromDate, query.ToDate);
        int availableRoomsCount = await availableRooms.CountAsync();

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