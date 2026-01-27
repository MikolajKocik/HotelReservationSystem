using HotelReservationSystem.Application.CQRS.Abstractions.Commands;

namespace HotelReservationSystem.Application.CQRS.Rooms.Commands;

/// <summary>
/// Command to update an existing room
/// </summary>
public record UpdateRoomCommand(
    int Id,
    decimal PricePerNight,
    bool IsAvailable,
    string? ImagePath = null
) : ICommand;