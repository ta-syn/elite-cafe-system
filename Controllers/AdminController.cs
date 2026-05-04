using CafeManagement.Data;
using CafeManagement.Filters;
using CafeManagement.Models;
using CafeManagement.Models.Enums;
using CafeManagement.Models.ViewModels;
using CafeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.Controllers
{
    [SessionAuthorize("Admin")]
    public class AdminController : Controller
    {
        private readonly MenuService  _menuService;
        private readonly OrderService _orderService;

        public AdminController(MenuService menuService, OrderService orderService)
        {
            _menuService  = menuService;
            _orderService = orderService;
        }

        // ── DASHBOARD ─────────────────────────
        public IActionResult Dashboard()
        {
            // ═══ OOP CONCEPT: EXCEPTION HANDLING ═══
            try
            {
                var allOrders = _orderService.GetAllOrders();
                var allItems  = _menuService.GetAllItems();

                var vm = new DashboardViewModel
                {
                    TotalOrders    = allOrders.Count,
                    TotalRevenue   = allOrders
                        .Where(o => o.Status != OrderStatus.Cancelled)
                        .Sum(o => o.TotalAmount),
                    TotalMenuItems = allItems.Count,
                    PendingOrders  = allOrders.Count(o => o.Status == OrderStatus.Pending),
                    RecentOrders   = allOrders.Take(8).ToList()
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] Dashboard Error: {ex.Message}");
                TempData["Error"] = "Dashboard load হয়নি।";
                return View(new DashboardViewModel());
            }
        }

        // ── MENU LIST ─────────────────────────
        public IActionResult MenuList(string? search)
        {
            try
            {
                // ✅ FIXED: includeHidden: true — Admin সব items দেখবে
                var items = string.IsNullOrEmpty(search)
                    ? _menuService.GetAllItems()
                    : _menuService.Search(search, includeHidden: true);

                ViewBag.Search = search;
                return View(items);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<MenuItem>());
            }
        }

        // ── ADD ITEM ──────────────────────────
        [HttpGet]
        public IActionResult AddItem() => View(new AddMenuItemViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ CSRF protection
        public IActionResult AddItem(AddMenuItemViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            try
            {
                // ═══ OOP CONCEPT: POLYMORPHISM ═══
                MenuItem item = vm.ItemType switch
                {
                    "Beverage" => new Beverage
                    {
                        Name        = vm.Name,
                        Price       = vm.Price,
                        Description = vm.Description,
                        ImageEmoji  = vm.ImageEmoji,
                        IsAvailable = vm.IsAvailable,
                        IsHot       = vm.IsHot,
                        Size        = vm.Size
                    },
                    "Food" => new Food
                    {
                        Name            = vm.Name,
                        Price           = vm.Price,
                        Description     = vm.Description,
                        ImageEmoji      = vm.ImageEmoji,
                        IsAvailable     = vm.IsAvailable,
                        PrepTimeMinutes = vm.PrepTimeMinutes,
                        IsVegetarian    = vm.IsVegetarian
                    },
                    _ => throw new ArgumentException("Invalid item type.")
                };

                _menuService.AddItem(item);
                TempData["Success"] = $"'{vm.Name}' যোগ করা হয়েছে! ✅";
                return RedirectToAction("MenuList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        // ── EDIT ITEM ─────────────────────────
        [HttpGet]
        public IActionResult EditItem(int id)
        {
            try
            {
                var item = _menuService.GetById(id);
                if (item == null)
                {
                    TempData["Error"] = "Item পাওয়া যায়নি।";
                    return RedirectToAction("MenuList");
                }

                var vm = new AddMenuItemViewModel
                {
                    ItemType    = item is Beverage ? "Beverage" : "Food",
                    Name        = item.Name,
                    Price       = item.Price,
                    Description = item.Description,
                    ImageEmoji  = item.ImageEmoji,
                    IsAvailable = item.IsAvailable
                };

                if (item is Beverage bev)
                {
                    vm.IsHot = bev.IsHot;
                    vm.Size  = bev.Size;
                }
                else if (item is Food food)
                {
                    vm.PrepTimeMinutes = food.PrepTimeMinutes;
                    vm.IsVegetarian    = food.IsVegetarian;
                }

                ViewBag.ItemId = id;
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("MenuList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ CSRF protection
        public IActionResult EditItem(int id, AddMenuItemViewModel vm)
        {
            if (!ModelState.IsValid) { ViewBag.ItemId = id; return View(vm); }

            try
            {
                MenuItem item = vm.ItemType switch
                {
                    "Beverage" => new Beverage
                    {
                        Id          = id,
                        Name        = vm.Name,
                        Price       = vm.Price,
                        Description = vm.Description,
                        ImageEmoji  = vm.ImageEmoji,
                        IsAvailable = vm.IsAvailable,
                        IsHot       = vm.IsHot,
                        Size        = vm.Size
                    },
                    "Food" => new Food
                    {
                        Id              = id,
                        Name            = vm.Name,
                        Price           = vm.Price,
                        Description     = vm.Description,
                        ImageEmoji      = vm.ImageEmoji,
                        IsAvailable     = vm.IsAvailable,
                        PrepTimeMinutes = vm.PrepTimeMinutes,
                        IsVegetarian    = vm.IsVegetarian
                    },
                    _ => throw new ArgumentException("Invalid item type.")
                };

                _menuService.UpdateItem(item);
                TempData["Success"] = "Item update হয়েছে! ✅";
                return RedirectToAction("MenuList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        // ── DELETE ITEM ───────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ CSRF protection
        public IActionResult DeleteItem(int id)
        {
            try
            {
                _menuService.DeleteItem(id);
                TempData["Success"] = "Item delete হয়েছে।";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("MenuList");
        }

        // ── TOGGLE AVAILABILITY ───────────────
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ CSRF protection
        public IActionResult ToggleAvailability(int id)
        {
            try
            {
                _menuService.ToggleAvailability(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("MenuList");
        }

        // ── ALL ORDERS ────────────────────────
        public IActionResult AllOrders(string? status)
        {
            try
            {
                var orders = _orderService.GetAllOrders();

                if (!string.IsNullOrEmpty(status) &&
                    Enum.TryParse<OrderStatus>(status, out var s))
                    orders = orders.Where(o => o.Status == s).ToList();

                ViewBag.StatusFilter = status;
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<Order>());
            }
        }

        // ── ORDER DETAIL ──────────────────────
        public IActionResult OrderDetail(int id)
        {
            try
            {
                var order = _orderService.GetOrderById(id);
                if (order == null)
                {
                    TempData["Error"] = "Order পাওয়া যায়নি।";
                    return RedirectToAction("AllOrders");
                }
                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("AllOrders");
            }
        }

        // ── UPDATE ORDER STATUS ───────────────
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ CSRF protection
        public IActionResult UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                if (!Enum.TryParse<OrderStatus>(status, out var orderStatus))
                    throw new ArgumentException("Invalid status.");

                _orderService.UpdateStatus(orderId, orderStatus);
                TempData["Success"] = "Status update হয়েছে! ✅";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("OrderDetail", new { id = orderId });
        }
    }
}
