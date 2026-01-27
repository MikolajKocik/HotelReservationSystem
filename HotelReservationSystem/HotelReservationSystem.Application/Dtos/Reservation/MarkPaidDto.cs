namespace HotelReservationSystem.Application.Dtos.Reservation;

public class MarkPaidDto
{
    public string ReservationId { get; set; } = default!;
    public string PaymentIntentId { get; set; } = default!;
}