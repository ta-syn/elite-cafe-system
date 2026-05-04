namespace CafeManagement.Models
{
    public class OrderItem
    {
        public int MenuItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        // Computed property
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
