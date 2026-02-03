using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.QueryHandlers;

/// <summary>
/// Handler for retrieving a reservation by ID
/// </summary>
public sealed class GetReservationByIdQueryHandler : IQueryHandler<GetReservationByIdQuery, ReservationDto?>
{
    private readonly IReservationRepository reservationRepository;

    public GetReservationByIdQueryHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the query to get a reservation by ID
    /// </summary>
    public async Task<ReservationDto?> HandleAsync(GetReservationByIdQuery query, CancellationToken cancellationToken = default)
    {
        Reservation? reservation = await this.reservationRepository.GetByIdAsync(query.Id, cancellationToken);

        if (reservation == null)
        {
            return null;
        }

        return reservation.ToDto();
    }
}