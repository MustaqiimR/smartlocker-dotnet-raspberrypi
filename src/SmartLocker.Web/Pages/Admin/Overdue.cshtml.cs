using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class OverdueModel : PageModel
    {
        private readonly BorrowService _borrowService;
        private readonly LogService _logService;

        public List<SmartLocker.Web.Models.Borrow> OverdueBorrows { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public OverdueModel(BorrowService borrowService, LogService logService)
        {
            _borrowService = borrowService;
            _logService = logService;
        }

        public void OnGet()
        {
            // Update overdue status first
            _borrowService.UpdateOverdueStatus();
            
            // Get overdue borrows
            OverdueBorrows = _borrowService.GetOverdueBorrows();
        }

        public IActionResult OnPostSendReminder(int borrowId)
        {
            try
            {
                var borrow = _borrowService.GetBorrowById(borrowId);
                if (borrow == null)
                {
                    ErrorMessage = "Borrow not found.";
                    return RedirectToPage();
                }

                // Log the reminder action (actual email/notification would be sent here in Phase 5+)
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "SendReminder", "Borrow", borrowId, $"Sent overdue reminder for borrow {borrowId} to user {borrow.User?.FullName}", "Success");

                SuccessMessage = $"Reminder sent to {borrow.User?.FullName}.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error sending reminder: {ex.Message}";
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "SendReminder", "Borrow", borrowId, ex.Message, "Failed");
                return RedirectToPage();
            }
        }

        public IActionResult OnPostForceReturn(int borrowId)
        {
            try
            {
                var borrow = _borrowService.GetBorrowById(borrowId);
                if (borrow == null)
                {
                    ErrorMessage = "Borrow not found.";
                    return RedirectToPage();
                }

                _borrowService.ReturnBorrow(borrowId);

                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "ForceReturnOverdue", "Borrow", borrowId, $"Force returned overdue borrow {borrowId}", "Success");

                SuccessMessage = $"Overdue item returned successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error returning item: {ex.Message}";
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "ForceReturnOverdue", "Borrow", borrowId, ex.Message, "Failed");
                return RedirectToPage();
            }
        }
    }
}
