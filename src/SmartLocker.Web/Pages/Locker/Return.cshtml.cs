using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Web.Pages.Locker
{
    public class ReturnModel : PageModel
    {
        private readonly SmartLockerDbContext _context;
        private readonly BorrowService _borrowService;
        private readonly LogService _logService;

        public List<Borrow> ActiveBorrows { get; set; } = new();
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public ReturnModel(SmartLockerDbContext context, BorrowService borrowService, LogService logService)
        {
            _context = context;
            _borrowService = borrowService;
            _logService = logService;
        }

        public void OnGet()
        {
            // Load all active borrows for the kiosk display
            var activeBorrowStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Active");
            if (activeBorrowStatus != null)
            {
                ActiveBorrows = _context.Borrows
                    .Where(b => b.BorrowStatusId == activeBorrowStatus.BorrowStatusId)
                    .Include(b => b.Item)
                    .Include(b => b.User)
                    .ToList();
            }
        }

        public async Task<IActionResult> OnPostAsync(int borrowId, string condition, string notes)
        {
            try
            {
                var borrow = await _context.Borrows
                    .Include(b => b.Item)
                    .Include(b => b.User)
                    .FirstOrDefaultAsync(b => b.BorrowId == borrowId);

                if (borrow == null)
                {
                    ErrorMessage = "Borrow record not found.";
                    OnGet();
                    return Page();
                }

                if (borrow.BorrowStatus?.BorrowStatusName != "Active")
                {
                    ErrorMessage = "This item is not currently borrowed.";
                    OnGet();
                    return Page();
                }

                // Update borrow status
                var returnedStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Returned");
                if (returnedStatus != null)
                {
                    borrow.BorrowStatusId = returnedStatus.BorrowStatusId;
                }
                borrow.BorrowReturnDate = DateTime.UtcNow;

                // Update item status based on condition
                if (borrow.Item != null)
                {
                    var statusMap = new Dictionary<string, string>
                    {
                        { "Lost", "Lost" },
                        { "Damaged", "Maintenance" },
                        { "Good", "Available" }
                    };
                    
                    if (statusMap.TryGetValue(condition, out var newStatus))
                    {
                        var itemStatus = _context.ItemStatuses.FirstOrDefault(s => s.ItemStatusName == newStatus);
                        if (itemStatus != null)
                        {
                            borrow.Item.ItemStatusId = itemStatus.ItemStatusId;
                        }
                    }
                }

                // Update locker status
                if (borrow.Item?.LockerId.HasValue == true && borrow.Item.LockerId > 0)
                {
                    var locker = await _context.Lockers.FindAsync(borrow.Item.LockerId.Value);
                    if (locker != null)
                    {
                        var availableStatus = _context.LockerStatuses.FirstOrDefault(s => s.LockerStatusName == "Available");
                        if (availableStatus != null)
                        {
                            locker.LockerStatusId = availableStatus.LockerStatusId;
                        }
                    }
                }

                // Save changes
                await _context.SaveChangesAsync();

                // Log the return
                _logService.LogAction(
                    borrow.UserId,
                    "Return Item",
                    "Borrow",
                    borrow.BorrowId,
                    $"Item '{borrow.Item?.ItemName}' returned. Condition: {condition}. Notes: {notes ?? "None"}"
                );

                SuccessMessage = $"Item '{borrow.Item?.ItemName}' has been successfully returned!";
                OnGet();
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error processing return: {ex.Message}";
                OnGet();
                return Page();
            }
        }
    }
}
