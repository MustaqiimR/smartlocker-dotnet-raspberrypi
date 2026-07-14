using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Admin
{
    public class ItemBorrowHistoryModel : PageModel
    {
        private readonly BorrowLogService _borrowLogService;
        private readonly ItemService _itemService;

        public ItemBorrowHistoryModel(BorrowLogService borrowLogService, ItemService itemService)
        {
            _borrowLogService = borrowLogService;
            _itemService = itemService;
        }

        public Item Item { get; set; }
        public List<BorrowLog> BorrowHistory { get; set; } = new List<BorrowLog>();
        public BorrowItemStatsDto Stats { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int itemId)
        {
            // Get item details
            Item = _itemService.GetItemById(itemId);
            if (Item == null)
            {
                ErrorMessage = "Item not found";
                return NotFound();
            }

            // Get borrow history
            BorrowHistory = await _borrowLogService.GetItemBorrowHistoryAsync(itemId);

            // Get statistics
            Stats = await _borrowLogService.GetItemBorrowStatsAsync(itemId);

            return Page();
        }
    }
}
