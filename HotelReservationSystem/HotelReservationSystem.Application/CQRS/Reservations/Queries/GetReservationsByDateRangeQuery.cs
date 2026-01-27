using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;

namespace HotelReservationSystem.Application.CQRS.Reservations.Queries;

/// <summary>
/// Query to retrieve reservations within a specific date range
/// </summary>
public record GetReservationsByDateRangeQuery(
    DateTime FromDate,
    DateTime ToDate
) : IQuery<IQueryable<ReservationDto>>;
