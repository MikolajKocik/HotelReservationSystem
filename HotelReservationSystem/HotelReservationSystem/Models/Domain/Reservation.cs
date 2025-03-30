namespace HotelReservationSystem.Models.Domain
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Status { get; set; } = default!;
       
        public int RoomId { get; set; }
        public Room Room { get; set; } = default!;

        public int GuestId { get; set; }
        public Guest Guest { get; set; } = default!;

        public Payment Payment { get; set; } = default!;
    }
}
