using CafeManagement.Data;
using CafeManagement.Models;
using CafeManagement.Models.Enums;

namespace CafeManagement.Services
{
    public class AuthService
    {
        // ✅ UPDATED: JsonDataStore এর বদলে AppDbContext
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        // Login check
        public User? Login(string email, string password)
        {
            // ═══ OOP CONCEPT: EXCEPTION HANDLING ═══
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Email এবং Password দিতে হবে।");

                // ✅ UPDATED: Password hash করে compare করা হচ্ছে
                var hashedPassword = AppDbContext.HashPassword(password);

                return _db.Users.FirstOrDefault(u =>
                    u.Email.ToLower() == email.ToLower() &&
                    u.Password == hashedPassword);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] Login Error: {ex.Message}");
                return null;
            }
        }

        // Register new customer
        public bool Register(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("সব field পূরণ করতে হবে।");

                // Email already exists?
                if (_db.Users.Any(u => u.Email.ToLower() == email.ToLower()))
                    throw new InvalidOperationException("এই email দিয়ে আগেই account আছে।");

                var newUser = new User
                {
                    Name = name,
                    Email = email,
                    Password = AppDbContext.HashPassword(password), // ✅ hashed
                    Role = UserRole.Customer
                };

                _db.Users.Add(newUser);
                _db.SaveChanges();
                return true;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] Register Error: {ex.Message}");
                return false;
            }
        }

        // Get user from session id
        public User? GetUserById(int id)
        {
            try
            {
                return _db.Users.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] GetUser Error: {ex.Message}");
                return null;
            }
        }
    }
}
