using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Application.CQRS.Abstractions.Queries;

namespace HotelReservationSystem.Application.CQRS.Payments.Queries;

public record GetTransactionsQuery : IQuery<IEnumerable<Payment>>;
