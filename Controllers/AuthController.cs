using CafeManagement.Filters;
using CafeManagement.Models.ViewModels;
using CafeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // ── LOGIN ──────────────────────────────
        [HttpGet]
        [Route("Auth/Login")]
        [Route("Login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserId") != null)
                return RedirectToAction("Index", "Menu");

            return View(new LoginViewModel());
        }

        [HttpPost]
        [Route("Auth/Login")]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            // ... (rest of logic)
            if (!ModelState.IsValid) return View(model);

            try
            {
                var user = _authService.Login(model.Email, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Email বা Password ভুল।");
                    return View(model);
                }

                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserRole", user.Role.ToString());

                if (user.Role == Models.Enums.UserRole.Admin)
                    return RedirectToAction("Dashboard", "Admin");

                return RedirectToAction("Index", "Menu");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // ── REGISTER ──────────────────────────
        [HttpGet]
        [Route("Auth/Register")]
        [Route("Register")]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserId") != null)
                return RedirectToAction("Index", "Menu");

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [Route("Auth/Register")]
        [Route("Register")]
        [ValidateAntiForgeryToken] // ✅ CSRF protection
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                _authService.Register(model.Name, model.Email, model.Password);

                var user = _authService.Login(model.Email, model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserRole", user.Role.ToString());
                }

                TempData["Success"] = "Account তৈরি হয়েছে! স্বাগতম ☕";
                return RedirectToAction("Index", "Menu");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // ── LOGOUT ────────────────────────────
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
