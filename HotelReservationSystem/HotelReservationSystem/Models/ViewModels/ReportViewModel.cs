namespace HotelReservationSystem.Models.ViewModels
{
    public class ReportViewModel
    {
        public int TotalReservations { get; set; }
        public int ConfirmedReservations { get; set; }
        public int CanceledReservations { get; set; }
        public decimal TotalPayments { get; set; }
        public int AvailableRooms { get; set; }
    }
}
