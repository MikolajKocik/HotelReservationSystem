using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Reports.Queries;
using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Infrastructure.CQRS.Reports.QueryHandlers;

/// <summary>
/// Handler for generating reports
/// </summary>
public sealed class GenerateReportQueryHandler : IQueryHandler<GenerateReportQuery, ReportDto>
{
    private readonly IReservationRepository reservationRepository;
    private readonly IRoomRepository roomRepository;
    private readonly IPaymentRepository paymentRepository;

    public GenerateReportQueryHandler(
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository,
        IPaymentRepository paymentRepository)
    {
        this.reservationRepository = reservationRepository;
        this.roomRepository = roomRepository;
        this.paymentRepository = paymentRepository;
    }

    /// <summary>
    /// Handles the query to generate a report
    /// </summary>
    public async Task<ReportDto> HandleAsync(GenerateReportQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<Reservation> reservations = await this.reservationRepository.GetByDateRangeAsync(query.FromDate, query.ToDate, cancellationToken);

        int totalReservations = reservations.Count();
        int confirmedReservations = reservations.Count(r => r.Status == ReservationStatus.Confirmed);
        int canceledReservations = reservations.Count(r => r.Status == ReservationStatus.Cancelled);
        
        List<Payment> allPayments = await this.paymentRepository.GetAllAsync(cancellationToken);
        // Sumuj płatności - zarówno Paid jak i inne zakończone sukcesem
        decimal totalPayments = allPayments
            .Where(p => p.Status == PaymentStatus.Paid || p.Status == PaymentStatus.Pending)
            .Sum(p => p.Amount);

        IEnumerable<Room> availableRooms = await this.roomRepository.GetAvailableRoomsAsync(query.FromDate, query.ToDate, cancellationToken);
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