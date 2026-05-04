using CafeManagement.Models.Enums;
using CafeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuService _menuService;

        public MenuController(MenuService menuService)
        {
            _menuService = menuService;
        }

        public IActionResult Index(string? category, string? search)
        {
            try
            {
                var items = string.IsNullOrEmpty(search)
                    ? (string.IsNullOrEmpty(category)
                        ? _menuService.GetAvailableItems()
                        : Enum.TryParse<ItemCategory>(category, out var cat)
                            ? _menuService.GetByCategory(cat)
                            : _menuService.GetAvailableItems())
                    : _menuService.Search(search); // Customer: includeHidden=false (default)

                ViewBag.Category = category;
                ViewBag.Search   = search;
                return View(items);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<CafeManagement.Models.MenuItem>());
            }
        }
    }
}
