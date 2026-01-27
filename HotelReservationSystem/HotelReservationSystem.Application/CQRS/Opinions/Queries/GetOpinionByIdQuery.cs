using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Opinion;

namespace HotelReservationSystem.Application.CQRS.Opinions.Queries;

public record GetOpinionByIdQuery(string OpinionId) : IQuery<OpinionDto?>;