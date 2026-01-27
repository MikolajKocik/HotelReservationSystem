using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Rooms.Commands;

/// <summary>
/// Command to toggle room availability
/// </summary>
public record ToggleRoomAvailabilityCommand(
    int RoomId
) : ICommand;