using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;

namespace HotelReservationSystem.Application.CQRS.Reservations.Queries;

/// <summary>
/// Query to retrieve all reservations with related data
/// </summary>
public record GetAllReservationsQuery : IQuery<IQueryable<ReservationDto>>;
