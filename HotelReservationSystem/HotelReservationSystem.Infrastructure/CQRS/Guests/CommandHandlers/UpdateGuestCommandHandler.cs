using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Guests.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Guests.CommandHandlers;

/// <summary>
/// Handler for updating an existing guest
/// </summary>
public sealed class UpdateGuestCommandHandler : ICommandHandler<UpdateGuestCommand>
{
    private readonly IGuestRepository guestRepository;

    public UpdateGuestCommandHandler(IGuestRepository guestRepository)
    {
        this.guestRepository = guestRepository;
    }

    /// <summary>
    /// Handles the command to update a guest
    /// </summary>
    public async Task HandleAsync(UpdateGuestCommand command, CancellationToken cancellationToken = default)
    {
        Guest? guest = await this.guestRepository.GetByIdAsync(command.Id, cancellationToken);
        
        if (guest == null)
            throw new InvalidOperationException($"Guest with ID {command.Id} not found");

        guest.UpdateContactInfo(command.Email, command.PhoneNumber);
        
        await this.guestRepository.UpdateAsync(guest, cancellationToken);
    }
}
