using System.ComponentModel.DataAnnotations;

namespace CafeManagement.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email দাও")]
        [EmailAddress(ErrorMessage = "Valid email দাও")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password দাও")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
