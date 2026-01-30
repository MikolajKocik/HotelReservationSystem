using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Payments.Commands;

public record RefusePaymentCommand(string PaymentIntentId) : ICommand;
