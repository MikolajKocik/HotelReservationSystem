using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Opinions.Commands;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Infrastructure.CQRS.Opinions.CommandHandlers;

public sealed class CreateOpinionCommandHandler : ICommandHandler<CreateOpinionCommand, string>
{
    private readonly IOpinionRepository opinionRepository;
    private readonly IReservationRepository reservationRepository;
    private readonly IGuestRepository guestRepository;

    public CreateOpinionCommandHandler(
        IOpinionRepository opinionRepository,
        IReservationRepository reservationRepository,
        IGuestRepository guestRepository)
    {
        this.opinionRepository = opinionRepository;
        this.reservationRepository = reservationRepository;
        this.guestRepository = guestRepository;
    }

    public async Task<string> HandleAsync(CreateOpinionCommand request, CancellationToken cancellationToken = default)
    {
        Guest? guest = await this.guestRepository.GetByEmailAsync(request.UserEmail, cancellationToken);
        if (guest == null)
        {
            throw new Exception("Guest not found");
        }

        Reservation? reservation = await this.reservationRepository.GetByIdAsync(request.OpinionDto.ReservationId, cancellationToken);
        if (reservation == null)
        {
            throw new Exception("Reservation not found");
        }

        if (reservation.Status != ReservationStatus.Confirmed && reservation.Status != ReservationStatus.Completed)
        {
            throw new Exception("Can only add opinion to confirmed or completed reservations");
        }

        Opinion? existingOpinion = await this.opinionRepository.GetByReservationIdAsync(request.OpinionDto.ReservationId, cancellationToken);
        if (existingOpinion != null)
        {
            throw new Exception("Opinion already exists for this reservation");
        }

        var opinion = new Opinion(
            request.OpinionDto.Rating,
            request.OpinionDto.Comment,
            request.OpinionDto.ReservationId,
            guest.Id
        );

        await this.opinionRepository.AddAsync(opinion, cancellationToken);
        return opinion.Id;
    }
}