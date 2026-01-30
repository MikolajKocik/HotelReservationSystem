using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Opinions.Queries;
using HotelReservationSystem.Application.Dtos.Opinion;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Opinions.QueryHandlers;

public sealed class GetOpinionsQueryHandler : IQueryHandler<GetOpinionsQuery, IEnumerable<OpinionDto>>
{
    private readonly IOpinionRepository opinionRepository;

    public GetOpinionsQueryHandler(IOpinionRepository opinionRepository)
    {
        this.opinionRepository = opinionRepository;
    }

    public async Task<IEnumerable<OpinionDto>> HandleAsync(GetOpinionsQuery request, CancellationToken cancellationToken)
    {
        var opinions = await this.opinionRepository.GetAllAsync();
        return opinions.Select(o => new OpinionDto
        {
            Id = o.Id,
            Rating = o.Rating,
            Comment = o.Comment,
            CreatedAt = o.CreatedAt,
            ReservationId = o.ReservationId,
            GuestId = o.GuestId,
            GuestFirstName = o.Guest.FirstName,
            GuestLastName = o.Guest.LastName,
            RoomId = o.Reservation.RoomId,
            RoomNumber = o.Reservation.Room.Number
        });
    }
}