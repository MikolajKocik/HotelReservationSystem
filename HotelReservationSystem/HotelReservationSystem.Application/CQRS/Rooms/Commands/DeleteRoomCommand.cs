using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Rooms.Commands;

/// <summary>
/// Command to delete a room
/// </summary>
public record DeleteRoomCommand(
    int Id
) : ICommand;