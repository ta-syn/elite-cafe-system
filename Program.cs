using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── SQLite Database ────────────────────────────────
builder.Services.AddDbContext<CafeManagement.Data.AppDbContext>(options =>
    options.UseSqlite("Data Source=cafe.db"));

// ── Services ──────────────────────────────────────
builder.Services.AddScoped<CafeManagement.Services.AuthService>();
builder.Services.AddScoped<CafeManagement.Services.MenuService>();
builder.Services.AddScoped<CafeManagement.Services.OrderService>();

// ── Session ───────────────────────────────────────
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ── Database auto-create on startup ───────────────
// প্রথমবার run করলে cafe.db file তৈরি হবে
// seed data ঢুকবে (admin user + menu items)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<CafeManagement.Data.AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
