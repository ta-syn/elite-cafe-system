using CafeManagement.Data;
using CafeManagement.Models;
using CafeManagement.Models.Enums;

namespace CafeManagement.Services
{
    public class MenuService
    {
        // ✅ UPDATED: AppDbContext use করা হচ্ছে
        private readonly AppDbContext _db;

        public MenuService(AppDbContext db)
        {
            _db = db;
        }

        public List<MenuItem> GetAllItems()
        {
            try
            {
                return _db.MenuItems.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] GetAllItems Error: {ex.Message}");
                return new List<MenuItem>();
            }
        }

        public List<MenuItem> GetAvailableItems()
        {
            try
            {
                return _db.MenuItems.Where(m => m.IsAvailable).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] GetAvailableItems Error: {ex.Message}");
                return new List<MenuItem>();
            }
        }

        public List<MenuItem> GetByCategory(ItemCategory category)
        {
            try
            {
                return _db.MenuItems
                    .Where(m => m.Category == category && m.IsAvailable)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] GetByCategory Error: {ex.Message}");
                return new List<MenuItem>();
            }
        }

        public MenuItem? GetById(int id)
        {
            try
            {
                return _db.MenuItems.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] GetById Error: {ex.Message}");
                return null;
            }
        }

        // ✅ FIXED: includeHidden parameter যোগ করা হয়েছে
        // Customer এর জন্য includeHidden = false (default)
        // Admin এর জন্য includeHidden = true
        public List<MenuItem> Search(string query, bool includeHidden = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return includeHidden ? GetAllItems() : GetAvailableItems();

                var q = query.ToLower();
                return _db.MenuItems
                    .Where(m => (includeHidden || m.IsAvailable) &&
                        (m.Name.ToLower().Contains(q) ||
                         m.Description.ToLower().Contains(q)))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] Search Error: {ex.Message}");
                return new List<MenuItem>();
            }
        }

        public void AddItem(MenuItem item)
        {
            try
            {
                _db.MenuItems.Add(item);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] AddItem Error: {ex.Message}");
                throw new Exception("Item add করা যায়নি।");
            }
        }

        public void UpdateItem(MenuItem item)
        {
            try
            {
                _db.MenuItems.Update(item);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] UpdateItem Error: {ex.Message}");
                throw new Exception("Item update করা যায়নি।");
            }
        }

        public void DeleteItem(int id)
        {
            try
            {
                var item = _db.MenuItems.Find(id);
                if (item != null)
                {
                    _db.MenuItems.Remove(item);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] DeleteItem Error: {ex.Message}");
                throw new Exception("Item delete করা যায়নি।");
            }
        }

        public void ToggleAvailability(int id)
        {
            try
            {
                var item = _db.MenuItems.Find(id);
                if (item == null) throw new Exception("Item পাওয়া যায়নি।");
                item.IsAvailable = !item.IsAvailable;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] Toggle Error: {ex.Message}");
                throw;
            }
        }
    }
}
