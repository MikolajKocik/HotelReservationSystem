using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Hasła się nie zgadzają.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

}
