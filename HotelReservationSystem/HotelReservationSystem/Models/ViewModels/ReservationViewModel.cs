namespace HotelReservationSystem.Models.ViewModels
{
    public class ReservationViewModel
    {
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }

        public int RoomId { get; set; }

        public string GuestFirstName { get; set; } = default!;
        public string GuestLastName { get; set; } = default!;
        public string GuestEmail { get; set; } = default!;
        public string GuestPhoneNumber { get; set; } = default!;
    }
}
