using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Web.ViewModels;

public record LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; init; } = string.Empty;

    public bool RememberMe { get; init; }
}
