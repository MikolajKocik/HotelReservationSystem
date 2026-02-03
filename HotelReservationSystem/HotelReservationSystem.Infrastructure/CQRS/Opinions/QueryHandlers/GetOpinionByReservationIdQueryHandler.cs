using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Opinions.Queries;
using HotelReservationSystem.Application.Dtos.Opinion;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Opinions.QueryHandlers;

public sealed class GetOpinionByReservationIdQueryHandler : IQueryHandler<GetOpinionByReservationIdQuery, OpinionDto?>
{
    private readonly IOpinionRepository opinionRepository;

    public GetOpinionByReservationIdQueryHandler(IOpinionRepository opinionRepository)
    {
        this.opinionRepository = opinionRepository;
    }

    public async Task<OpinionDto?> HandleAsync(GetOpinionByReservationIdQuery request, CancellationToken cancellationToken = default)
    {
        var opinion = await this.opinionRepository.GetByReservationIdAsync(request.ReservationId, cancellationToken);
        if (opinion == null)
        {
            return null;
        }

        return opinion.ToDto();
    }
}