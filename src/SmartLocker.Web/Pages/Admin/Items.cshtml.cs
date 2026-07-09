using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ItemsModel : PageModel
    {
        private readonly ItemService _itemService;
        private readonly LockerService _lockerService;
        private readonly LogService _logService;
        private readonly SmartLocker.Web.Data.SmartLockerDbContext _context;

        public List<Item> Items { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<SmartLocker.Web.Models.Locker> Lockers { get; set; } = new();
        public Item EditingItem { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public ItemsModel(ItemService itemService, LockerService lockerService, LogService logService, SmartLocker.Web.Data.SmartLockerDbContext context)
        {
            _itemService = itemService;
            _lockerService = lockerService;
            _logService = logService;
            _context = context;
        }

        public void OnGet(int? editId)
        {
            if (editId.HasValue)
            {
                EditingItem = _itemService.GetItemById(editId.Value);
            }
            LoadData();
        }

        public IActionResult OnPost(string action, int itemId, string itemName, string description, int categoryId, int lockerId)
        {
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            try
            {
                if (action == "create")
                {
                    if (string.IsNullOrEmpty(itemName) || categoryId == 0)
                    {
                        ErrorMessage = "Item name and category are required.";
                    }
                    else
                    {
                        var item = _itemService.CreateItem(itemName, description, categoryId, lockerId == 0 ? (int?)null : lockerId);
                        _logService.LogAction(currentUserId, "Create", "Item", item.ItemId, $"Created item: {itemName}", "Success");
                        SuccessMessage = $"Item '{itemName}' created successfully.";
                    }
                }
                else if (action == "edit")
                {
                    if (string.IsNullOrEmpty(itemName) || categoryId == 0)
                    {
                        ErrorMessage = "Item name and category are required.";
                    }
                    else
                    {
                        var item = _itemService.GetItemById(itemId);
                        if (item != null)
                        {
                            item.ItemName = itemName;
                            item.Description = description;
                            item.CategoryId = categoryId;
                            item.LockerId = lockerId == 0 ? (int?)null : lockerId;
                            item.UpdatedAt = DateTime.UtcNow;
                            _context.Items.Update(item);
                            _context.SaveChanges();
                            _logService.LogAction(currentUserId, "Edit", "Item", itemId, $"Updated item: {itemName}", "Success");
                            SuccessMessage = $"Item '{itemName}' updated successfully.";
                            EditingItem = null;
                        }
                        else
                        {
                            ErrorMessage = "Item not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                _logService.LogAction(currentUserId, action, "Item", itemId, ex.Message, "Failed");
            }

            LoadData();
            return Page();
        }

        private void LoadData()
        {
            Items = _itemService.GetAllItems();
            Categories = _context.Categories.ToList();
            Lockers = _lockerService.GetAllLockers();
        }
    }
}
