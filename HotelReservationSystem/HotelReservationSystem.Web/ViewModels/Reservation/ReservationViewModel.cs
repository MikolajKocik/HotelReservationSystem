using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Web.ViewModels
{
    public class ReservationViewModel
    {
        [Required(ErrorMessage = "Data przyjazdu jest wymagana.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data przyjazdu")]
        public DateTime ArrivalDate { get; set; }

        [Required(ErrorMessage = "Data wyjazdu jest wymagana.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data wyjazdu")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Pokój jest wymagany.")]
        [Display(Name = "Pokój")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Imię gościa jest wymagane.")]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków.")]
        [Display(Name = "Imię")]
        public string GuestFirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko gościa jest wymagane.")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków.")]
        [Display(Name = "Nazwisko")]
        public string GuestLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email gościa jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
        [Display(Name = "Email")]
        public string GuestEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu.")]
        [Display(Name = "Telefon")]
        public string GuestPhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Kod rabatowy")]
        public string? DiscountCode { get; set; } 

        [Display(Name = "Uwagi do rezerwacji")]
        public string? AdditionalRequests { get; init; } = string.Empty;

        [Display(Name = "Suma całkowita")]
        public decimal TotalSum => 
            (DepartureDate - ArrivalDate).Days * GetRoomPrice(RoomId) * GetDiscountMultiplier(DiscountCode);
    
        private decimal GetRoomPrice(int roomId)
        {
            return roomId switch
            {
                1 => 550m, 
                2 => 700m, 
                _ => 0m
            };
        }

        private decimal GetDiscountMultiplier(string? discountCode)
        {
            return discountCode?.ToUpper() switch
            {
                "SUMMER10" => 0.9m,
                "WINTER15" => 0.85m,
                _ => 1.0m
            };
        }
    }
}
