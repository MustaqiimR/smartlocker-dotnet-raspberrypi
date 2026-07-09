using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Staff
{
    [Authorize(Roles = "Staff,Admin")]
    public class ManageBorrowsModel : PageModel
    {
        private readonly BorrowService _borrowService;
        private readonly LogService _logService;

        public ManageBorrowsModel(BorrowService borrowService, LogService logService)
        {
            _borrowService = borrowService;
            _logService = logService;
        }

        public List<Borrow> UserActiveBorrows { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            UserActiveBorrows = _borrowService.GetUserActiveBorrows(userId);
        }
    }
}
