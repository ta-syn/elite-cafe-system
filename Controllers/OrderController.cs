using CafeManagement.Filters;
using CafeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.Controllers
{
    [SessionAuthorize]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult History()
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId")!);
                var orders = _orderService.GetOrdersByUser(userId);
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<CafeManagement.Models.Order>());
            }
        }

        public IActionResult Detail(int id)
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId")!);
                var order  = _orderService.GetOrderById(id);

                if (order == null || order.UserId != userId)
                {
                    TempData["Error"] = "Order পাওয়া যায়নি।";
                    return RedirectToAction("History");
                }

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("History");
            }
        }

        public IActionResult Confirmation(int id)
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId")!);
                var order  = _orderService.GetOrderById(id);

                if (order == null || order.UserId != userId)
                    return RedirectToAction("History");

                return View(order);
            }
            catch
            {
                return RedirectToAction("History");
            }
        }
    }
}
