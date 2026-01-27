using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.Dtos.Opinion;

namespace HotelReservationSystem.Application.CQRS.Opinions.Commands;

public record UpdateOpinionCommand(string UserEmail, UpdateOpinionDto OpinionDto) : ICommand;