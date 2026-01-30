using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Web.ViewModels;

public record RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; init; } = string.Empty;

    [Compare("Password", ErrorMessage = "Hasła się nie zgadzają.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; init; } = string.Empty;
}
