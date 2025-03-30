namespace HotelReservationSystem.Models.Domain
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public string Status { get; set; } = default!;

        public int ReservationId { get; set; }
    }
}
