using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Guests.Commands;
using HotelReservationSystem.Core.Domain.Entities.GuestPref;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Guests.CommandHandlers;

public sealed class SaveGuestPreferenceCommandHandler : ICommandHandler<SaveGuestPreferenceCommand>
{
    private readonly IGuestRepository _repository;

    public SaveGuestPreferenceCommandHandler(IGuestRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(SaveGuestPreferenceCommand command, CancellationToken cancellationToken = default)
    {
        var preference = new GuestPreference(
            command.email,
            command.category,
            command.value
        );

        await _repository.AddGuestPreferenceAsync(preference, cancellationToken);
    }
}
