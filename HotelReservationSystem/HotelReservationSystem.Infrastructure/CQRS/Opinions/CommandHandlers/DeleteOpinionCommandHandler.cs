using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Opinions.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Opinions.CommandHandlers;

public sealed class DeleteOpinionCommandHandler : ICommandHandler<DeleteOpinionCommand>
{
    private readonly IOpinionRepository opinionRepository;
    private readonly IGuestRepository guestRepository;

    public DeleteOpinionCommandHandler(
        IOpinionRepository opinionRepository,
        IGuestRepository guestRepository)
    {
        this.opinionRepository = opinionRepository;
        this.guestRepository = guestRepository;
    }

    public async Task HandleAsync(DeleteOpinionCommand request, CancellationToken cancellationToken)
    {
        Guest? guest = await this.guestRepository.GetByEmailAsync(request.UserEmail);
        if (guest == null)
        {
            throw new Exception("Guest not found");
        }

        Opinion? opinion = await this.opinionRepository.GetByIdAsync(request.OpinionId);
        if (opinion == null)
        {
            throw new Exception("Opinion not found");
        }

        if (opinion.GuestId != guest.Id)
        {
            throw new Exception("Opinion does not belong to the user");
        }

        await this.opinionRepository.DeleteAsync(request.OpinionId);
    }
}