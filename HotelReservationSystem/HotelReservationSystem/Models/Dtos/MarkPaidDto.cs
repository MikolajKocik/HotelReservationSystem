namespace HotelReservationSystem.Models.Dtos
{
    public class MarkPaidDto
    {
        public int ReservationId { get; set; }
        public string PaymentIntentId { get; set; } = default!;
    }
}
