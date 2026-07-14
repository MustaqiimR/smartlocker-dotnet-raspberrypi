using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Staff
{
    [Authorize(Roles = "Staff,Admin")]
    public class BorrowHistoryModel : PageModel
    {
        private readonly BorrowService _borrowService;

        public List<Borrow> BorrowHistory { get; set; } = new();
        public string SelectedStatus { get; set; } = "All";

        public BorrowHistoryModel(BorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        public void OnGet(string status = "All")
        {
            SelectedStatus = status;
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            var allBorrows = _borrowService.GetUserBorrows(currentUserId);
            
            if (status == "Active")
            {
                BorrowHistory = allBorrows.Where(b => b.BorrowStatusId == 1).ToList(); // Active
            }
            else if (status == "Returned")
            {
                BorrowHistory = allBorrows.Where(b => b.BorrowStatusId == 2).ToList(); // Returned
            }
            else if (status == "Overdue")
            {
                BorrowHistory = allBorrows.Where(b => b.BorrowStatusId == 3).ToList(); // Overdue
            }
            else
            {
                BorrowHistory = allBorrows;
            }
            
            BorrowHistory = BorrowHistory.OrderByDescending(b => b.BorrowStartDate).ToList();
        }
    }
}
