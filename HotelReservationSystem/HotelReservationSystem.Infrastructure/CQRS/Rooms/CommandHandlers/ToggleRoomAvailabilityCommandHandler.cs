using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Rooms.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.CommandHandlers;

/// <summary>
/// Handler for toggling room availability
/// </summary>
public sealed class ToggleRoomAvailabilityCommandHandler : ICommandHandler<ToggleRoomAvailabilityCommand>
{
    private readonly IRoomRepository roomRepository;

    public ToggleRoomAvailabilityCommandHandler(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    /// <summary>
    /// Handles the command to toggle room availability
    /// </summary>
    public async Task HandleAsync(ToggleRoomAvailabilityCommand command, CancellationToken cancellationToken = default)
    {
        await this.roomRepository.ToggleAvailabilityAsync(command.RoomId, cancellationToken);
    }
}