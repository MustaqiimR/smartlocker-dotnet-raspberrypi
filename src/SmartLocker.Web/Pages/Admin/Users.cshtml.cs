using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly SmartLocker.Web.Data.SmartLockerDbContext _context;

        public List<User> Users { get; set; } = new();
        public List<Role> Roles { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public UsersModel(UserService userService, LogService logService, SmartLocker.Web.Data.SmartLockerDbContext context)
        {
            _userService = userService;
            _logService = logService;
            _context = context;
        }

        public void OnGet()
        {
            LoadData();
        }

        public IActionResult OnPost(string action, int userId, string username, string password, string fullName, string email, int roleId)
        {
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            try
            {
                if (action == "create")
                {
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName))
                    {
                        ErrorMessage = "Username, password, and full name are required.";
                    }
                    else
                    {
                        var user = _userService.CreateUser(username, password, fullName, email, roleId);
                        _logService.LogAction(currentUserId, "Create", "User", user.UserId, $"Created user: {username}", "Success");
                        SuccessMessage = $"User '{username}' created successfully.";
                    }
                }
                else if (action == "disable")
                {
                    _userService.DisableUser(userId);
                    _logService.LogAction(currentUserId, "Disable", "User", userId, $"Disabled user ID: {userId}", "Success");
                    SuccessMessage = "User disabled successfully.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                _logService.LogAction(currentUserId, action, "User", userId, ex.Message, "Failed");
            }

            LoadData();
            return Page();
        }

        private void LoadData()
        {
            Users = _userService.GetAllUsers();
            Roles = _context.Roles.ToList();
        }
    }
}
