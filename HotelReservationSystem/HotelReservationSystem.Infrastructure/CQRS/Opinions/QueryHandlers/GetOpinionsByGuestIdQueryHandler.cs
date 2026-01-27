using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Opinions.Queries;
using HotelReservationSystem.Application.Dtos.Opinion;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Opinions.QueryHandlers;

public class GetOpinionsByGuestIdQueryHandler : IQueryHandler<GetOpinionsByGuestIdQuery, IEnumerable<OpinionDto>>
{
    private readonly IOpinionRepository _opinionRepository;

    public GetOpinionsByGuestIdQueryHandler(IOpinionRepository opinionRepository)
    {
        _opinionRepository = opinionRepository;
    }

    public async Task<IEnumerable<OpinionDto>> HandleAsync(GetOpinionsByGuestIdQuery request, CancellationToken cancellationToken)
    {
        var opinions = await _opinionRepository.GetByGuestIdAsync(request.GuestId);
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