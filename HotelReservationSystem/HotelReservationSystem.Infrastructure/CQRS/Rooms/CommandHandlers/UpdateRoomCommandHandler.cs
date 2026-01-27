using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Rooms.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.CommandHandlers;

/// <summary>
/// Handler for updating an existing room
/// </summary>
public class UpdateRoomCommandHandler : ICommandHandler<UpdateRoomCommand>
{
    private readonly IRoomRepository roomRepository;

    public UpdateRoomCommandHandler(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    /// <summary>
    /// Handles the command to update a room
    /// </summary>
    public async Task HandleAsync(UpdateRoomCommand command, CancellationToken cancellationToken = default)
    {
        Room? room = await roomRepository.GetByIdAsync(command.Id);
        if (room != null)
        {
            room.UpdatePrice(command.PricePerNight);
            room.SetAvailability(command.IsAvailable);
            room.UpdateImage(command.ImagePath);
            await roomRepository.UpdateAsync(room);
        }
    }
}