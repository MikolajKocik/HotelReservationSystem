using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.CQRS.Rooms.Commands;

/// <summary>
/// Command to create a new room
/// </summary>
public record CreateRoomCommand(
    string Number,
    RoomType Type,
    decimal PricePerNight,
    string? ImagePath = null
) : ICommand<int>;