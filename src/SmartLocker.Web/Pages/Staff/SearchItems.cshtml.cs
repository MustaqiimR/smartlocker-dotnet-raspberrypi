using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Staff
{
    [Authorize(Roles = "Staff,Admin")]
    public class SearchItemsModel : PageModel
    {
        private readonly ItemService _itemService;
        private readonly SmartLocker.Web.Data.SmartLockerDbContext _context;

        public List<Item> Items { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<ItemStatus> ItemStatuses { get; set; } = new();
        public string SearchTerm { get; set; }
        public int SelectedCategoryId { get; set; }
        public int SelectedStatusId { get; set; }

        public SearchItemsModel(ItemService itemService, SmartLocker.Web.Data.SmartLockerDbContext context)
        {
            _itemService = itemService;
            _context = context;
        }

        public void OnGet(string searchTerm, int categoryId, int statusId)
        {
            SearchTerm = searchTerm;
            SelectedCategoryId = categoryId;
            SelectedStatusId = statusId;

            Categories = _context.Categories.ToList();
            ItemStatuses = _context.ItemStatuses.ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Items = _itemService.SearchItems(searchTerm);
            }
            else
            {
                Items = _itemService.GetAllItems();
            }

            if (categoryId > 0)
            {
                Items = Items.Where(i => i.CategoryId == categoryId).ToList();
            }

            if (statusId > 0)
            {
                Items = Items.Where(i => i.ItemStatusId == statusId).ToList();
            }
        }
    }
}
