using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;

namespace HotelReservationSystem.Application.CQRS.Reservations.Queries;

/// <summary>
/// Query to retrieve a specific reservation by ID
/// </summary>
public record GetReservationByIdQuery(string Id) : IQuery<ReservationDto?>;
