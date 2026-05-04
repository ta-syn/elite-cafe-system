namespace CafeManagement.Models
{
    // Session এ store হবে, DB তে না
    public class CartItem
    {
        public int MenuItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageEmoji { get; set; } = "☕";

        public decimal Subtotal => Quantity * UnitPrice;
    }
}
