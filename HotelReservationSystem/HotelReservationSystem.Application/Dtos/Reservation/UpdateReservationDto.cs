using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Reservation;

public record UpdateReservationDto
{
    public string ReservationId { get; init; } = string.Empty;
    public ReservationStatus Status { get; init; }
    public string Reason { get; init; } = string.Empty;
}