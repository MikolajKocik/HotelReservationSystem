using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Guests.Commands;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Guests.CommandHandlers;

/// <summary>
/// Handler for creating a new guest
/// </summary>
public sealed class CreateGuestCommandHandler : ICommandHandler<CreateGuestCommand, string>
{
    private readonly IGuestRepository guestRepository;

    public CreateGuestCommandHandler(IGuestRepository guestRepository)
    {
        this.guestRepository = guestRepository;
    }

    /// <summary>
    /// Handles the command to create a new guest
    /// </summary>
    public async Task<string> HandleAsync(CreateGuestCommand command, CancellationToken cancellationToken = default)
    {
        var guest = new Guest(
            command.FirstName,
            command.LastName,
            command.Email,
            command.PhoneNumber
        );

        return await this.guestRepository.CreateAsync(guest);
    }
}
