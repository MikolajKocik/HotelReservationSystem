using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Web.ViewModels;

public record ReservationViewModel
{
        [Required(ErrorMessage = "Data przyjazdu jest wymagana.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data przyjazdu")]
        public DateTime ArrivalDate { get; init; }

        [Required(ErrorMessage = "Data wyjazdu jest wymagana.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data wyjazdu")]
        public DateTime DepartureDate { get; init; }

        [Required(ErrorMessage = "Pokój jest wymagany.")]
        [Display(Name = "Pokój")]
        public int RoomId { get; init; }
        [Required(ErrorMessage = "Imię gościa jest wymagane.")]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków.")]
        [Display(Name = "Imię")]
        public string GuestFirstName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko gościa jest wymagane.")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków.")]
        [Display(Name = "Nazwisko")]
        public string GuestLastName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Email gościa jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
        [Display(Name = "Email")]
        public string GuestEmail { get; init; } = string.Empty;

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu.")]
        [Display(Name = "Telefon")]
        public string GuestPhoneNumber { get; init; } = string.Empty;

        [Display(Name = "Kod rabatowy")]
        public string? DiscountCode { get; init; } 

        [Display(Name = "Uwagi do rezerwacji")]
        public string? AdditionalRequests { get; init; } = string.Empty;

        [Display(Name = "Zgoda na przetwarzanie danych osobowych")]
        public bool AcceptPrivacy { get; init; }

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
