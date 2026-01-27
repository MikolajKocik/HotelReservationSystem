using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Opinions.Queries;
using HotelReservationSystem.Application.Dtos.Opinion;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Opinions.QueryHandlers;

public class GetOpinionByReservationIdQueryHandler : IQueryHandler<GetOpinionByReservationIdQuery, OpinionDto?>
{
    private readonly IOpinionRepository _opinionRepository;

    public GetOpinionByReservationIdQueryHandler(IOpinionRepository opinionRepository)
    {
        _opinionRepository = opinionRepository;
    }

    public async Task<OpinionDto?> HandleAsync(GetOpinionByReservationIdQuery request, CancellationToken cancellationToken)
    {
        var opinion = await _opinionRepository.GetByReservationIdAsync(request.ReservationId);
        if (opinion == null)
        {
            return null;
        }

        return new OpinionDto
        {
            Id = opinion.Id,
            Rating = opinion.Rating,
            Comment = opinion.Comment,
            CreatedAt = opinion.CreatedAt,
            ReservationId = opinion.ReservationId,
            GuestId = opinion.GuestId,
            GuestFirstName = opinion.Guest.FirstName,
            GuestLastName = opinion.Guest.LastName,
            RoomId = opinion.Reservation.RoomId,
            RoomNumber = opinion.Reservation.Room.Number
        };
    }
}