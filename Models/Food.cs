using CafeManagement.Models.Enums;

namespace CafeManagement.Models
{
    // ═══ OOP CONCEPT: INHERITANCE ═══
    // ═══ OOP CONCEPT: POLYMORPHISM ═══

    public class Food : MenuItem
    {
        public int PrepTimeMinutes { get; set; } = 10;
        public bool IsVegetarian { get; set; } = false;

        public Food()
        {
            Category = ItemCategory.Food;
            ImageEmoji = "🍽️";
            ItemType = "Food";
        }

        // Override — POLYMORPHISM
        public override string GetDescription()
        {
            return $"{Name} ({(IsVegetarian ? "Veg" : "Non-Veg")}) — Ready in {PrepTimeMinutes} mins";
        }
    }
}
