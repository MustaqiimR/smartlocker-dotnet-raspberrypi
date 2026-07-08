using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly LogService _logService;

        public LogoutModel(LogService logService)
        {
            _logService = logService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                _logService.LogAction(int.Parse(userId), "Logout", "User", int.Parse(userId), $"User {username} logged out", "Success");
            }

            await HttpContext.SignOutAsync("SmartLockerAuth");
            return RedirectToPage("/Auth/Login");
        }
    }
}
