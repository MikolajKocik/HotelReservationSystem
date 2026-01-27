using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Reservation;

public class UpdateReservationDto
{
    public string ReservationId { get; set; } = default!;
    public ReservationStatus Status { get; set; }
    public string Reason { get; set; } = string.Empty;
}