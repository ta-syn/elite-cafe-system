using System.ComponentModel.DataAnnotations;

namespace CafeManagement.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "নাম দাও")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email দাও")]
        [EmailAddress(ErrorMessage = "Valid email দাও")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password দাও")]
        [MinLength(6, ErrorMessage = "কমপক্ষে 6 character")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password আবার দাও")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password মিলছে না")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
