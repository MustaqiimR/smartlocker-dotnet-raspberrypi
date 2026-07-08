using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Staff
{
    [Authorize(Roles = "Staff,Admin")]
    public class DashboardModel : PageModel
    {
        private readonly RequestService _requestService;
        private readonly BorrowService _borrowService;
        private readonly ItemService _itemService;

        public int PendingRequests { get; set; }
        public int ActiveBorrows { get; set; }
        public int OverdueItems { get; set; }
        public int AvailableItems { get; set; }

        public DashboardModel(RequestService requestService, BorrowService borrowService, ItemService itemService)
        {
            _requestService = requestService;
            _borrowService = borrowService;
            _itemService = itemService;
        }

        public void OnGet()
        {
            PendingRequests = _requestService.GetPendingRequests().Count;
            
            var activeBorrows = _borrowService.GetActiveBorrows();
            ActiveBorrows = activeBorrows.Count;
            
            var overdueBorrows = _borrowService.GetOverdueBorrows();
            OverdueItems = overdueBorrows.Count;
            
            AvailableItems = _itemService.GetAvailableItems().Count;
        }
    }
}
