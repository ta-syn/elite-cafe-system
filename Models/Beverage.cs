using CafeManagement.Models.Enums;

namespace CafeManagement.Models
{
    // ═══ OOP CONCEPT: INHERITANCE ═══
    // Beverage, MenuItem এর সব কিছু পাচ্ছে
    // ═══ OOP CONCEPT: POLYMORPHISM ═══
    // GetDescription() আলাদাভাবে কাজ করছে

    public class Beverage : MenuItem
    {
        public bool IsHot { get; set; } = true;
        public string Size { get; set; } = "Medium";

        public Beverage()
        {
            Category = ItemCategory.Beverage;
            ImageEmoji = "☕";
            ItemType = "Beverage";
        }

        // Override — POLYMORPHISM
        public override string GetDescription()
        {
            return $"{Size} {(IsHot ? "Hot" : "Cold")} {Name} — {Description}";
        }
    }
}
