using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class BorrowsModel : PageModel
    {
        private readonly BorrowService _borrowService;
        private readonly LogService _logService;

        public List<SmartLocker.Web.Models.Borrow> ActiveBorrows { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public BorrowsModel(BorrowService borrowService, LogService logService)
        {
            _borrowService = borrowService;
            _logService = logService;
        }

        public void OnGet()
        {
            ActiveBorrows = _borrowService.GetActiveBorrows();
        }

        public IActionResult OnPostExtend(int borrowId, int extensionDays)
        {
            try
            {
                var borrow = _borrowService.GetBorrowById(borrowId);
                if (borrow == null)
                {
                    ErrorMessage = "Borrow not found.";
                    return RedirectToPage();
                }

                var newDueDate = borrow.BorrowEndDate.AddDays(extensionDays);
                _borrowService.ExtendLoan(borrowId, newDueDate);

                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "ExtendLoan", "Borrow", borrowId, $"Extended loan by {extensionDays} days. New due date: {newDueDate}", "Success");

                SuccessMessage = $"Loan extended by {extensionDays} days.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error extending loan: {ex.Message}";
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "ExtendLoan", "Borrow", borrowId, ex.Message, "Failed");
                return RedirectToPage();
            }
        }

        public IActionResult OnPostReturn(int borrowId)
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
                _logService.LogAction(int.Parse(currentUserId), "ForceReturn", "Borrow", borrowId, $"Force returned borrow {borrowId}", "Success");

                SuccessMessage = $"Item returned successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error returning item: {ex.Message}";
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "ForceReturn", "Borrow", borrowId, ex.Message, "Failed");
                return RedirectToPage();
            }
        }

        public IActionResult OnPostMarkLost(int borrowId)
        {
            try
            {
                var borrow = _borrowService.GetBorrowById(borrowId);
                if (borrow == null)
                {
                    ErrorMessage = "Borrow not found.";
                    return RedirectToPage();
                }

                _borrowService.MarkAsLost(borrowId);

                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "MarkLost", "Borrow", borrowId, $"Marked borrow {borrowId} as lost", "Success");

                SuccessMessage = $"Item marked as lost.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error marking item as lost: {ex.Message}";
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "MarkLost", "Borrow", borrowId, ex.Message, "Failed");
                return RedirectToPage();
            }
        }
    }
}
