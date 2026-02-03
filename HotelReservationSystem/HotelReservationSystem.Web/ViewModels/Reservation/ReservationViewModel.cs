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
    public DateTime ArrivalDate { get; init; }

    [Required(ErrorMessage = "Data wyjazdu jest wymagana.")]
    public DateTime DepartureDate { get; init; }

    [Required(ErrorMessage = "Liczba gości jest wymagana.")]
    [Range(1, 4, ErrorMessage = "Liczba gości musi wynosić od 1 do 4.")]
    public int NumberOfGuests { get; init; } = 1; 

    [Required(ErrorMessage = "Pokój jest wymagany.")]
    public int RoomId { get; init; }

    [Required(ErrorMessage = "Imię gościa jest wymagane.")]
    [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków.")]
    [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź' \-]+$", ErrorMessage = "Imię może zawierać tylko litery, spacje, apostrofy i myślniki.")]
    public string GuestFirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Nazwisko gościa jest wymagane.")]
    [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków.")]
    [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź' \-]+$", ErrorMessage = "Nazwisko może zawierać tylko litery, spacje, apostrofy i myślniki.")]
    public string GuestLastName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Email gościa jest wymagany.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Wprowadź poprawny adres email (np. user@domain.com).")]
    public string GuestEmail { get; init; } = string.Empty;

    [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
    [RegularExpression(@"^\+?\d{9,}$", ErrorMessage = "Numer telefonu musi zawierać co najmniej 9 cyfr (opcjonalnie z prefiksem '+').")]
    public string GuestPhoneNumber { get; init; } = string.Empty;


    public string? DiscountCode { get; init; }

    public string? AdditionalRequests { get; init; } = string.Empty;

    [Required(ErrorMessage = "Akceptacja polityki prywatności jest wymagana.")]
    public bool AcceptPrivacy { get; init; }

    public decimal? TotalSum { get; init; }

    public string FormType { get; init; } = "single";
}
