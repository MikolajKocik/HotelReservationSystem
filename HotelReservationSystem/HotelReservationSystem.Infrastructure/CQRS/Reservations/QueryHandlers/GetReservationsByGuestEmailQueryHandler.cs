using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.QueryHandlers;

/// <summary>
/// Handler for retrieving reservations by guest email
/// </summary>
public sealed class GetReservationsByGuestEmailQueryHandler : IQueryHandler<GetReservationsByGuestEmailQuery, IEnumerable<ReservationDto>>
{
    private readonly IReservationRepository reservationRepository;

    public GetReservationsByGuestEmailQueryHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the query to get reservations by guest email
    /// </summary>
    public async Task<IEnumerable<ReservationDto>> HandleAsync(GetReservationsByGuestEmailQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<Reservation> reservations = await this.reservationRepository.GetByGuestEmailAsync(query.Email, cancellationToken);

        return reservations.Select(r => r.ToDto()).ToList();
    }
}