using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Rooms.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.CommandHandlers;

/// <summary>
/// Handler for deleting a room
/// </summary>
public sealed class DeleteRoomCommandHandler : ICommandHandler<DeleteRoomCommand>
{
    private readonly IRoomRepository roomRepository;

    public DeleteRoomCommandHandler(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    /// <summary>
    /// Handles the command to delete a room
    /// </summary>
    public async Task HandleAsync(DeleteRoomCommand command, CancellationToken cancellationToken = default)
    {
        await this.roomRepository.DeleteAsync(command.Id);
    }
}