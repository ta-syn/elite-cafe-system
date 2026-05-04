// ═══ OOP CONCEPT: INTERFACE ═══
// সব MenuItem কে এই contract follow করতে হবে

namespace CafeManagement.Models.Interfaces
{
    public interface IOrderable
    {
        string Name { get; set; }
        decimal Price { get; set; }
        string GetDescription();  // প্রতিটা item নিজের description দেবে
    }
}
