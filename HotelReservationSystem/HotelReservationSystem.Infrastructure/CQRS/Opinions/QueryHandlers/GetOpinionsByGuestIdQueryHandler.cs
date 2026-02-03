using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Opinions.Queries;
using HotelReservationSystem.Application.Dtos.Opinion;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Opinions.QueryHandlers;

public sealed class GetOpinionsByGuestIdQueryHandler : IQueryHandler<GetOpinionsByGuestIdQuery, IEnumerable<OpinionDto>>
{
    private readonly IOpinionRepository opinionRepository;

    public GetOpinionsByGuestIdQueryHandler(IOpinionRepository opinionRepository)
    {
        this.opinionRepository = opinionRepository;
    }

    public async Task<IEnumerable<OpinionDto>> HandleAsync(GetOpinionsByGuestIdQuery request, CancellationToken cancellationToken = default)
    {
        var opinions = await this.opinionRepository.GetByGuestIdAsync(request.GuestId, cancellationToken);
        return opinions.Select(o => o.ToDto());
    }
}