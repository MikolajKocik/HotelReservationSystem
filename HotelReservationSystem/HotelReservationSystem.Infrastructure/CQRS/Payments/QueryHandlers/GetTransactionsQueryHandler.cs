using HotelReservationSystem.Application.CQRS.Payments.Queries;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Application.CQRS.Abstractions.Queries;

namespace HotelReservationSystem.Infrastructure.CQRS.Payments.QueryHandlers;

public sealed class GetTransactionsQueryHandler : IQueryHandler<GetTransactionsQuery, IEnumerable<Payment>>
{
    private readonly IGuestRepository guestRepository;

    public GetTransactionsQueryHandler(IGuestRepository guestRepository)
    {
        this.guestRepository = guestRepository;
    }

    public async Task<IEnumerable<Payment>> HandleAsync(GetTransactionsQuery query, CancellationToken cancellationToken = default)
    {
        List<Payment> list = await this.guestRepository.GetTransactions(cancellationToken);
        return list ?? Enumerable.Empty<Payment>();
    }
}
