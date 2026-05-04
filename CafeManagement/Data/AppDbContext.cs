using Microsoft.EntityFrameworkCore;
using CafeManagement.Models;
using CafeManagement.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace CafeManagement.Data
{
    // ═══ DATABASE CONNECTION CLASS ═══
    // EF Core দিয়ে SQLite database manage করা হচ্ছে

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ═══ TABLES ═══
        public DbSet<User> Users { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ═══ OOP CONCEPT: POLYMORPHISM ═══
            // TPH (Table Per Hierarchy) — Beverage আর Food
            // একই MenuItems table 에 থাকবে।
            // ItemType column দিয়ে কোনটা Beverage/Food বোঝা যাবে।
            modelBuilder.Entity<MenuItem>()
                .HasDiscriminator(m => m.ItemType)
                .HasValue<Beverage>("Beverage")
                .HasValue<Food>("Food");

            // OrderItems আলাদা table এ store হবে
            modelBuilder.Entity<Order>()
                .OwnsMany(o => o.Items, oi =>
                {
                    oi.WithOwner().HasForeignKey("OrderId");
                    oi.Property<int>("Id");
                    oi.HasKey("Id");
                    oi.ToTable("OrderItems");
                });

            // ═══ SEED DATA ═══
            // Admin user — password SHA256 hashed
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@cafe.com",
                Password = HashPassword("admin123"),
                Role = UserRole.Admin
            });

            // Beverage seed data
            modelBuilder.Entity<Beverage>().HasData(
                new Beverage
                {
                    Id = 1, Name = "Espresso", Price = 150,
                    Description = "Strong Italian coffee",
                    IsHot = true, Size = "Small", IsAvailable = true,
                    ImageEmoji = "☕", Category = ItemCategory.Beverage,
                    ItemType = "Beverage"
                },
                new Beverage
                {
                    Id = 2, Name = "Latte", Price = 200,
                    Description = "Smooth milk coffee",
                    IsHot = true, Size = "Medium", IsAvailable = true,
                    ImageEmoji = "☕", Category = ItemCategory.Beverage,
                    ItemType = "Beverage"
                },
                new Beverage
                {
                    Id = 3, Name = "Cold Brew", Price = 220,
                    Description = "Cold steeped coffee",
                    IsHot = false, Size = "Large", IsAvailable = true,
                    ImageEmoji = "🧊", Category = ItemCategory.Beverage,
                    ItemType = "Beverage"
                }
            );

            // Food seed data
            modelBuilder.Entity<Food>().HasData(
                new Food
                {
                    Id = 4, Name = "Club Sandwich", Price = 280,
                    Description = "Triple layer sandwich",
                    IsVegetarian = false, PrepTimeMinutes = 10, IsAvailable = true,
                    ImageEmoji = "🥪", Category = ItemCategory.Food,
                    ItemType = "Food"
                },
                new Food
                {
                    Id = 5, Name = "Pasta", Price = 320,
                    Description = "Creamy white sauce",
                    IsVegetarian = true, PrepTimeMinutes = 15, IsAvailable = true,
                    ImageEmoji = "🍝", Category = ItemCategory.Food,
                    ItemType = "Food"
                },
                new Food
                {
                    Id = 6, Name = "Croissant", Price = 180,
                    Description = "Buttery French pastry",
                    IsVegetarian = true, PrepTimeMinutes = 5, IsAvailable = true,
                    ImageEmoji = "🥐", Category = ItemCategory.Food,
                    ItemType = "Food"
                }
            );
        }

        // ═══ PASSWORD HASHING — SHA256 ✅ ═══
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
