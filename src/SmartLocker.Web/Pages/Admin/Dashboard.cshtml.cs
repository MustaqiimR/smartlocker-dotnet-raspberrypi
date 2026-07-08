using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ItemService _itemService;
        private readonly BorrowService _borrowService;

        public int TotalUsers { get; set; }
        public int TotalItems { get; set; }
        public int ActiveBorrows { get; set; }
        public int OverdueItems { get; set; }

        public DashboardModel(UserService userService, ItemService itemService, BorrowService borrowService)
        {
            _userService = userService;
            _itemService = itemService;
            _borrowService = borrowService;
        }

        public void OnGet()
        {
            TotalUsers = _userService.GetAllUsers().Count;
            TotalItems = _itemService.GetAllItems().Count;
            
            var activeBorrows = _borrowService.GetActiveBorrows();
            ActiveBorrows = activeBorrows.Count;
            
            var overdueBorrows = _borrowService.GetOverdueBorrows();
            OverdueItems = overdueBorrows.Count;
        }
    }
}
