using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;

namespace HotelReservationSystem.Application.CQRS.Reservations.Queries;

/// <summary>
/// Query to retrieve reservations for a specific guest
/// </summary>
public record GetReservationsByGuestEmailQuery(string Email) : IQuery<IQueryable<ReservationDto>>;
