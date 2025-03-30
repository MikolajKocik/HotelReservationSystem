namespace HotelReservationSystem.Models.Domain
{
    public class Payment
    {
        public int Id { get; set; }
        public string Method { get; set; } = default!;
        public string Status { get; set; } = default!;
        public decimal Amount { get; set; }
        public string StripePaymentIntentId { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = default!;
    }
}
