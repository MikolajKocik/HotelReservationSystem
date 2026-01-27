using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Opinion;

namespace HotelReservationSystem.Application.CQRS.Opinions.Queries;

public record GetOpinionByReservationIdQuery(string ReservationId) : IQuery<OpinionDto?>;