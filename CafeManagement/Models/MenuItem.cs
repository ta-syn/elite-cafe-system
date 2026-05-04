using CafeManagement.Models.Enums;
using CafeManagement.Models.Interfaces;

namespace CafeManagement.Models
{
    // ═══ OOP CONCEPT: ABSTRACT CLASS ═══
    // সরাসরি new MenuItem() করা যাবে না
    // ═══ OOP CONCEPT: ENCAPSULATION ═══
    // Private fields, public properties দিয়ে control করা হয়েছে

    public abstract class MenuItem : IOrderable
    {
        // Private backing fields — ENCAPSULATION
        private string _name = string.Empty;
        private decimal _price;

        public int Id { get; set; }

        // EF Core TPH discriminator column এর জন্য
        public string ItemType { get; set; } = string.Empty;

        public string Name
        {
            get => _name;
            set
            {
                // ═══ OOP CONCEPT: EXCEPTION HANDLING ═══
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Item name cannot be empty.");
                _name = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public string Description { get; set; } = string.Empty;
        public string ImageEmoji { get; set; } = "☕";
        public bool IsAvailable { get; set; } = true;
        public ItemCategory Category { get; set; }

        // ═══ OOP CONCEPT: ABSTRACT METHOD ═══
        // Subclass অবশ্যই override করবে
        public abstract string GetDescription();
    }
}
