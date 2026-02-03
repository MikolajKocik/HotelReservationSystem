using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.QueryHandlers;

/// <summary>
/// Handler for retrieving reservations by date range
/// </summary>
public sealed class GetReservationsByDateRangeQueryHandler : IQueryHandler<GetReservationsByDateRangeQuery, IEnumerable<ReservationDto>>
{
    private readonly IReservationRepository reservationRepository;

    public GetReservationsByDateRangeQueryHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the query to get reservations by date range
    /// </summary>
    public async Task<IEnumerable<ReservationDto>> HandleAsync(GetReservationsByDateRangeQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<Reservation> reservations = await this.reservationRepository.GetByDateRangeAsync(query.FromDate, query.ToDate, cancellationToken);

        return reservations.Select(r => r.ToDto()).ToList();
    }
}