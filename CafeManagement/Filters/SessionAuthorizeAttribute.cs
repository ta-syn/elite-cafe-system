using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CafeManagement.Filters
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public SessionAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userId = session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (_roles.Length > 0)
            {
                var userRole = session.GetString("UserRole") ?? "";
                bool allowed = _roles.Any(r =>
                    string.Equals(r, userRole, StringComparison.OrdinalIgnoreCase));

                if (!allowed)
                {
                    context.Result = new RedirectToActionResult("Index", "Menu", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
