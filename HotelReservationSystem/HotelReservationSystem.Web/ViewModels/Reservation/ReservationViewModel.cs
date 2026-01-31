using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Web.ViewModels;

public record ReservationViewModel
{
    public ReservationViewModel()
    {
        ArrivalDate = DateTime.Today;
        DepartureDate = DateTime.Today.AddDays(1);
    }
    
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
    [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź' \-]+$", ErrorMessage = "Imię może zawierać tylko litery, spacje, apostrofy i myślniki.")]
    [Display(Name = "Imię")]
    public string GuestFirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Nazwisko gościa jest wymagane.")]
    [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków.")]
    [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź' \-]+$", ErrorMessage = "Nazwisko może zawierać tylko litery, spacje, apostrofy i myślniki.")]
    [Display(Name = "Nazwisko")]
    public string GuestLastName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Email gościa jest wymagany.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Wprowadź poprawny adres email (np. user@domain.com).")]
    [Display(Name = "Email")]
    public string GuestEmail { get; init; } = string.Empty;

    [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
    [RegularExpression(@"^\+?\d{9,}$", ErrorMessage = "Numer telefonu musi zawierać co najmniej 9 cyfr (opcjonalnie z prefiksem '+').")]
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

    [Display(Name = "Form Type")]
    public string FormType { get; init; } = "single";

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
