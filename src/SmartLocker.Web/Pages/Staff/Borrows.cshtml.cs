using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Staff
{
    [Authorize(Roles = "Staff,Admin")]
    public class BorrowsModel : PageModel
    {
        private readonly BorrowService _borrowService;
        private readonly LogService _logService;

        public List<SmartLocker.Web.Models.Borrow> UserActiveBorrows { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public BorrowsModel(BorrowService borrowService, LogService logService)
        {
            _borrowService = borrowService;
            _logService = logService;
        }

        public void OnGet()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
            UserActiveBorrows = _borrowService.GetUserActiveBorrows(int.Parse(currentUserId));
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

                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                
                // Verify user owns this borrow
                if (borrow.UserId != int.Parse(currentUserId))
                {
                    ErrorMessage = "You can only extend your own borrows.";
                    _logService.LogAction(int.Parse(currentUserId), "ExtendLoan", "Borrow", borrowId, "Unauthorized attempt to extend another user's borrow", "Failed");
                    return RedirectToPage();
                }

                var newDueDate = borrow.BorrowEndDate.AddDays(extensionDays);
                _borrowService.ExtendLoan(borrowId, newDueDate);

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

                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                
                // Verify user owns this borrow
                if (borrow.UserId != int.Parse(currentUserId))
                {
                    ErrorMessage = "You can only return your own borrows.";
                    _logService.LogAction(int.Parse(currentUserId), "ReturnBorrow", "Borrow", borrowId, "Unauthorized attempt to return another user's borrow", "Failed");
                    return RedirectToPage();
                }

                _borrowService.ReturnBorrow(borrowId);

                _logService.LogAction(int.Parse(currentUserId), "ReturnBorrow", "Borrow", borrowId, $"Returned borrow {borrowId}", "Success");

                SuccessMessage = $"Item returned successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error returning item: {ex.Message}";
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "ReturnBorrow", "Borrow", borrowId, ex.Message, "Failed");
                return RedirectToPage();
            }
        }
    }
}
