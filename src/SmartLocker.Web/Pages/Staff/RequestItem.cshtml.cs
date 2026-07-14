using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Staff
{
    [Authorize(Roles = "Staff,Admin")]
    public class RequestItemModel : PageModel
    {
        private readonly ItemService _itemService;
        private readonly RequestService _requestService;
        private readonly LogService _logService;
        private readonly BorrowLogService _borrowLogService;

        public Item Item { get; set; }
        public List<Item> Items { get; set; } = new();
        public List<BorrowLog> RecentBorrowHistory { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchTerm { get; set; }

        public RequestItemModel(ItemService itemService, RequestService requestService, LogService logService, BorrowLogService borrowLogService)
        {
            _itemService = itemService;
            _requestService = requestService;
            _logService = logService;
            _borrowLogService = borrowLogService;
        }

        public async Task OnGetAsync(int? id, string search = "")
        {
            SearchTerm = search;
            
            if (id.HasValue && id.Value > 0)
            {
                // Show specific item
                Item = _itemService.GetItemById(id.Value);
                
                // Load recent borrow history (last 5 entries)
                var allHistory = await _borrowLogService.GetItemBorrowHistoryAsync(id.Value);
                RecentBorrowHistory = allHistory.Take(5).ToList();
            }
            else
            {
                // Show list of available items
                Items = _itemService.GetAvailableItems();
                if (!string.IsNullOrEmpty(search))
                {
                    Items = Items.Where(i => i.ItemName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }
        }

        public IActionResult OnPost(int itemId, string justification, string requestedStartDate, string requestedEndDate)
        {
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            try
            {
                if (string.IsNullOrEmpty(justification))
                {
                    ErrorMessage = "Justification is required.";
                    Item = _itemService.GetItemById(itemId);
                    return Page();
                }

                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrEmpty(requestedStartDate) && DateTime.TryParse(requestedStartDate, out var start))
                {
                    startDate = start;
                }

                if (!string.IsNullOrEmpty(requestedEndDate) && DateTime.TryParse(requestedEndDate, out var end))
                {
                    endDate = end;
                }

                if (startDate.HasValue && endDate.HasValue && startDate >= endDate)
                {
                    ErrorMessage = "End date must be after start date.";
                    Item = _itemService.GetItemById(itemId);
                    return Page();
                }

                var request = _requestService.CreateRequest(currentUserId, itemId, justification, startDate, endDate);
                _logService.LogAction(currentUserId, "Create", "Request", request.RequestId, $"Created request for item {itemId}", "Success");
                
                SuccessMessage = "Request submitted successfully. Please wait for approval.";
                Item = _itemService.GetItemById(itemId);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                _logService.LogAction(currentUserId, "Create", "Request", itemId, ex.Message, "Failed");
                Item = _itemService.GetItemById(itemId);
            }

            return Page();
        }
    }
}
