using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Opinions.Queries;
using HotelReservationSystem.Application.Dtos.Opinion;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Opinions.QueryHandlers;

public sealed class GetOpinionByIdQueryHandler : IQueryHandler<GetOpinionByIdQuery, OpinionDto?>
{
    private readonly IOpinionRepository opinionRepository;

    public GetOpinionByIdQueryHandler(IOpinionRepository opinionRepository)
    {
        this.opinionRepository = opinionRepository;
    }

    public async Task<OpinionDto?> HandleAsync(GetOpinionByIdQuery request, CancellationToken cancellationToken = default)
    {
        var opinion = await this.opinionRepository.GetByIdAsync(request.OpinionId, cancellationToken);
        if (opinion == null)
        {
            return null;
        }

        return opinion.ToDto();
    }
}