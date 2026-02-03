using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Guests.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Guests.CommandHandlers;

/// <summary>
/// Handler for deleting a guest
/// </summary>
public sealed class DeleteGuestCommandHandler : ICommandHandler<DeleteGuestCommand>
{
    private readonly IGuestRepository guestRepository;

    public DeleteGuestCommandHandler(IGuestRepository guestRepository)
    {
        this.guestRepository = guestRepository;
    }

    /// <summary>
    /// Handles the command to delete a guest
    /// </summary>
    public async Task HandleAsync(DeleteGuestCommand command, CancellationToken cancellationToken = default)
    {
        await this.guestRepository.DeleteAsync(command.Id, cancellationToken);
    }
}
