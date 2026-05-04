using CafeManagement.Data;
using CafeManagement.Models;
using CafeManagement.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CafeManagement.Services
{
    public class OrderService
    {
        // ✅ UPDATED: AppDbContext use করা হচ্ছে
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db)
        {
            _db = db;
        }

        public Order PlaceOrder(int userId, string customerName, List<CartItem> cartItems)
        {
            // ═══ OOP CONCEPT: EXCEPTION HANDLING ═══
            try
            {
                if (cartItems == null || cartItems.Count == 0)
                    throw new InvalidOperationException("Cart empty, order দেওয়া যাবে না।");

                var orderItems = cartItems.Select(c => new OrderItem
                {
                    MenuItemId = c.MenuItemId,
                    ItemName   = c.ItemName,
                    UnitPrice  = c.UnitPrice,
                    Quantity   = c.Quantity
                }).ToList();

                var order = new Order
                {
                    UserId       = userId,
                    CustomerName = customerName,
                    Items        = orderItems,
                    TotalAmount  = cartItems.Sum(c => c.Subtotal),
                    Status       = OrderStatus.Pending,
                    CreatedAt    = DateTime.Now
                };

                _db.Orders.Add(order);
                _db.SaveChanges();
                return order;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] PlaceOrder Error: {ex.Message}");
                throw new Exception("Order দেওয়া যায়নি। আবার চেষ্টা করো।");
            }
        }

        public List<Order> GetOrdersByUser(int userId)
        {
            try
            {
                // ✅ Include() দিয়ে OrderItems load করা হচ্ছে
                return _db.Orders
                    .Include(o => o.Items)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedAt)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] GetOrdersByUser Error: {ex.Message}");
                return new List<Order>();
            }
        }

        public Order? GetOrderById(int id)
        {
            try
            {
                // ✅ Include() দিয়ে OrderItems load করা হচ্ছে
                return _db.Orders
                    .Include(o => o.Items)
                    .FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] GetOrderById Error: {ex.Message}");
                return null;
            }
        }

        public List<Order> GetAllOrders()
        {
            try
            {
                return _db.Orders
                    .Include(o => o.Items)
                    .OrderByDescending(o => o.CreatedAt)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] GetAllOrders Error: {ex.Message}");
                return new List<Order>();
            }
        }

        public void UpdateStatus(int orderId, OrderStatus status)
        {
            try
            {
                var order = _db.Orders.Find(orderId);
                if (order == null)
                    throw new InvalidOperationException("Order পাওয়া যায়নি।");

                order.Status = status;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] UpdateStatus Error: {ex.Message}");
                throw;
            }
        }
    }
}
