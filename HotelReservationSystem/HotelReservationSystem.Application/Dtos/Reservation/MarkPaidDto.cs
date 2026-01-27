namespace HotelReservationSystem.Application.Dtos.Reservation;

public record MarkPaidDto
{
    public string ReservationId { get; init; } = string.Empty;
    public string PaymentIntentId { get; init; } = string.Empty;
}