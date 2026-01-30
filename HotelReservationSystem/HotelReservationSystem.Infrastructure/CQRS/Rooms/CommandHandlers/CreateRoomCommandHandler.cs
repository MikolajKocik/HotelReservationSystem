using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Rooms.Commands;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Rooms.CommandHandlers;

/// <summary>
/// Handler for creating a new room
/// </summary>
public sealed class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, int>
{
    private readonly IRoomRepository roomRepository;

    public CreateRoomCommandHandler(IRoomRepository roomRepository)
    {
        this.roomRepository = roomRepository;
    }

    /// <summary>
    /// Handles the command to create a new room
    /// </summary>
    public async Task<int> HandleAsync(CreateRoomCommand command, CancellationToken cancellationToken = default)
    {
        var room = new Room(
            command.Number,
            command.Type,
            command.PricePerNight,
            "PLN", 
            command.ImagePath
        );

        return await this.roomRepository.CreateAsync(room);
    }
}