using System.Text.Json;
using CafeManagement.Filters;
using CafeManagement.Models;
using CafeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.Controllers
{
    public class CartController : Controller
    {
        private readonly MenuService _menuService;
        private readonly OrderService _orderService;

        public CartController(MenuService menuService, OrderService orderService)
        {
            _menuService  = menuService;
            _orderService = orderService;
        }

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(json)) return new List<CartItem>();
            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        // Cart count — [SessionAuthorize] ইচ্ছাকৃতভাবে নেই
        public IActionResult Count()
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return Json(new { count = 0 });

            var cart  = GetCart();
            int count = cart.Sum(c => c.Quantity);
            return Json(new { count });
        }

        [SessionAuthorize]
        public IActionResult Index()
        {
            var cart = GetCart();
            ViewBag.Total = cart.Sum(c => c.Subtotal);
            return View(cart);
        }

        [SessionAuthorize]
        [HttpPost]
        public IActionResult Add(int itemId, int quantity = 1)
        {
            try
            {
                var item = _menuService.GetById(itemId);
                if (item == null)
                    return Json(new { success = false, message = "Item পাওয়া যায়নি।" });

                var cart     = GetCart();
                var existing = cart.FirstOrDefault(c => c.MenuItemId == itemId);

                if (existing != null)
                    existing.Quantity += quantity;
                else
                    cart.Add(new CartItem
                    {
                        MenuItemId = item.Id,
                        ItemName   = item.Name,
                        UnitPrice  = item.Price,
                        Quantity   = quantity,
                        ImageEmoji = item.ImageEmoji
                    });

                SaveCart(cart);
                int total = cart.Sum(c => c.Quantity);
                return Json(new { success = true, cartCount = total });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [SessionAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ CSRF protection
        public IActionResult Remove(int itemId)
        {
            var cart = GetCart();
            cart.RemoveAll(c => c.MenuItemId == itemId);
            SaveCart(cart);
            TempData["Success"] = "Item সরানো হয়েছে।";
            return RedirectToAction("Index");
        }

        [SessionAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ CSRF protection
        public IActionResult UpdateQty(int itemId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MenuItemId == itemId);
            if (item != null)
            {
                if (quantity <= 0) cart.Remove(item);
                else item.Quantity = quantity;
            }
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        [SessionAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ CSRF protection
        public IActionResult Checkout()
        {
            try
            {
                var cart = GetCart();
                if (!cart.Any())
                {
                    TempData["Error"] = "Cart empty!";
                    return RedirectToAction("Index");
                }

                int    userId       = int.Parse(HttpContext.Session.GetString("UserId")!);
                string customerName = HttpContext.Session.GetString("UserName") ?? "Customer";

                var order = _orderService.PlaceOrder(userId, customerName, cart);

                HttpContext.Session.Remove("Cart");

                TempData["Success"] = $"Order #{order.Id} দেওয়া হয়েছে! ☕";
                return RedirectToAction("Confirmation", "Order", new { id = order.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
