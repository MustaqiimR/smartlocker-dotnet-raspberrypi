using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Staff
{
    [Authorize(Roles = "Staff,Admin")]
    public class ViewOverdueModel : PageModel
    {
        private readonly BorrowService _borrowService;
        private readonly LogService _logService;

        public ViewOverdueModel(BorrowService borrowService, LogService logService)
        {
            _borrowService = borrowService;
            _logService = logService;
        }

        public List<Borrow> OverdueBorrows { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var overdueBorrows = _borrowService.GetOverdueBorrows();
            OverdueBorrows = overdueBorrows.Where(b => b.UserId == userId).ToList();
        }

        public IActionResult OnPostReturn(int borrowId, string notes)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

            try
            {
                var borrow = _borrowService.GetBorrowById(borrowId);
                if (borrow == null || borrow.UserId != userId)
                {
                    ErrorMessage = "Borrow record not found or you don't have permission to return it.";
                    OnGet();
                    return Page();
                }

                _borrowService.ReturnBorrow(borrowId);
                _logService.LogAction(userId, "ReturnOverdueItem", "Borrow", borrowId, $"User {username} returned overdue item {borrow.Item?.ItemName}", "Success");
                SuccessMessage = "Overdue item returned successfully!";
                OnGet();
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error returning item: {ex.Message}";
                _logService.LogAction(userId, "ReturnOverdueItem", "Borrow", borrowId, $"Failed to return overdue item: {ex.Message}", "Failed");
                OnGet();
                return Page();
            }
        }
    }
}
