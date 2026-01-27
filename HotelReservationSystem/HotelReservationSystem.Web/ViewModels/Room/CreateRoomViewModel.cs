using System.ComponentModel.DataAnnotations;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Web.ViewModels.Room;

public class CreateRoomViewModel
{
    [Required(ErrorMessage = "Numer pokoju jest wymagany.")]
    [StringLength(10, ErrorMessage = "Numer pokoju nie może być dłuższy niż 10 znaków.")]
    [Display(Name = "Numer pokoju")]
    public string Number { get; set; } = string.Empty;

    [Required(ErrorMessage = "Typ pokoju jest wymagany.")]
    [Display(Name = "Typ pokoju")]
    public RoomType Type { get; set; }

    [Required(ErrorMessage = "Cena za noc jest wymagana.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena za noc musi być większa od zera.")]
    [Display(Name = "Cena za noc")]
    public decimal PricePerNight { get; set; }

    [Display(Name = "Zdjęcie pokoju")]
    public IFormFile? ImageFile { get; set; }
    public string? ImagePath { get; set; }
}
