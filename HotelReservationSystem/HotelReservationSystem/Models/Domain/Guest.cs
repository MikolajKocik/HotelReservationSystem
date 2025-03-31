namespace HotelReservationSystem.Models.Domain
{
    public class Guest
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
