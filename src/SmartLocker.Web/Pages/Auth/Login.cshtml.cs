using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Services;
using System.Security.Claims;

namespace SmartLocker.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly SmartLocker.Web.Services.AuthenticationService _authService;
        private readonly LogService _logService;

        public LoginModel(SmartLocker.Web.Services.AuthenticationService authService, LogService logService)
        {
            _authService = authService;
            _logService = logService;
        }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorMessage = "Username and password are required.";
                return Page();
            }

            var user = _authService.AuthenticateUser(username, password);

            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                _logService.LogAction(null, "Login", "User", null, $"Failed login attempt for user: {username}", "Failed");
                return Page();
            }

            // Create claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim("Email", user.Email ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "SmartLockerAuth");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                "SmartLockerAuth",
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logService.LogAction(user.UserId, "Login", "User", user.UserId, $"User {user.Username} logged in", "Success");

            // Redirect based on role
            if (user.Role.RoleName == "Admin")
            {
                return RedirectToPage("/Admin/Dashboard");
            }
            else
            {
                return RedirectToPage("/Staff/Dashboard");
            }
        }
    }
}
