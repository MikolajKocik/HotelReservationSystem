using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.QueryHandlers;

/// <summary>
/// Handler for retrieving all reservations
/// </summary>
public sealed class GetAllReservationsQueryHandler : IQueryHandler<GetAllReservationsQuery, IEnumerable<ReservationDto>>
{
    private readonly IReservationRepository reservationRepository;

    public GetAllReservationsQueryHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the query to get all reservations
    /// </summary>
    public async Task<IEnumerable<ReservationDto>> HandleAsync(GetAllReservationsQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<Reservation> reservations = await this.reservationRepository.GetAllAsync(cancellationToken);

        return reservations
            .Select(r => r.ToDto())
            .ToList();
    }
}