namespace HotelReservationSystem.Models.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; } = default!;
        public string Type { get; set; } = default!;
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }

        public string? Image { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
