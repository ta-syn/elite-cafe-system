using System.ComponentModel.DataAnnotations;
using CafeManagement.Models.Enums;

namespace CafeManagement.Models.ViewModels
{
    public class AddMenuItemViewModel
    {
        [Required(ErrorMessage = "Item type বেছে নাও")]
        public string ItemType { get; set; } = "Beverage";

        [Required(ErrorMessage = "নাম দাও")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "দাম দাও")]
        [Range(1, 99999, ErrorMessage = "1 থেকে 99999 এর মধ্যে হতে হবে")]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;
        public string ImageEmoji { get; set; } = "☕";
        public bool IsAvailable { get; set; } = true;

        // Beverage only
        public bool IsHot { get; set; } = true;
        public string Size { get; set; } = "Medium";

        // Food only
        public int PrepTimeMinutes { get; set; } = 10;
        public bool IsVegetarian { get; set; } = false;
    }
}
