using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Opinions.Commands;

public record DeleteOpinionCommand(string UserEmail, string OpinionId) : ICommand;