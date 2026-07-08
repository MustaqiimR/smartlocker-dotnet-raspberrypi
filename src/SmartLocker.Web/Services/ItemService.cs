using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Models;

namespace SmartLocker.Web.Services
{
    public class ItemService
    {
        private readonly SmartLockerDbContext _context;

        public ItemService(SmartLockerDbContext context)
        {
            _context = context;
        }

        public List<Item> GetAllItems()
        {
            return _context.Items
                .Include(i => i.Category)
                .Include(i => i.ItemStatus)
                .Include(i => i.Locker)
                .OrderBy(i => i.ItemName)
                .ToList();
        }

        public Item GetItemById(int itemId)
        {
            return _context.Items
                .Include(i => i.Category)
                .Include(i => i.ItemStatus)
                .Include(i => i.Locker)
                .FirstOrDefault(i => i.ItemId == itemId);
        }

        public List<Item> SearchItems(string searchTerm)
        {
            return _context.Items
                .Include(i => i.Category)
                .Include(i => i.ItemStatus)
                .Include(i => i.Locker)
                .Where(i => i.ItemName.Contains(searchTerm) || 
                            i.SerialNumber.Contains(searchTerm) ||
                            i.Category.CategoryName.Contains(searchTerm))
                .OrderBy(i => i.ItemName)
                .ToList();
        }

        public List<Item> GetAvailableItems()
        {
            var availableStatus = _context.ItemStatuses.FirstOrDefault(s => s.ItemStatusName == "Available");
            if (availableStatus == null)
            {
                return new List<Item>();
            }

            return _context.Items
                .Include(i => i.Category)
                .Include(i => i.ItemStatus)
                .Include(i => i.Locker)
                .Where(i => i.ItemStatusId == availableStatus.ItemStatusId)
                .OrderBy(i => i.ItemName)
                .ToList();
        }

        public Item CreateItem(string itemName, string description, int categoryId, int? lockerId)
        {
            var availableStatus = _context.ItemStatuses.FirstOrDefault(s => s.ItemStatusName == "Available");
            if (availableStatus == null)
            {
                throw new Exception("Available status not found");
            }

            var item = new Item
            {
                ItemName = itemName,
                Description = description,
                CategoryId = categoryId,
                ItemStatusId = availableStatus.ItemStatusId,
                LockerId = lockerId,
                SerialNumber = GenerateSerialNumber()
            };

            _context.Items.Add(item);
            _context.SaveChanges();

            return item;
        }

        public void UpdateItem(int itemId, string itemName, string description, int categoryId, int lockerId)
        {
            var item = GetItemById(itemId);
            if (item == null)
            {
                throw new Exception("Item not found");
            }

            item.ItemName = itemName;
            item.Description = description;
            item.CategoryId = categoryId;
            item.LockerId = lockerId;
            item.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void UpdateItemStatus(int itemId, string statusName)
        {
            var item = GetItemById(itemId);
            if (item == null)
            {
                throw new Exception("Item not found");
            }

            var status = _context.ItemStatuses.FirstOrDefault(s => s.ItemStatusName == statusName);
            if (status == null)
            {
                throw new Exception("Status not found");
            }

            item.ItemStatusId = status.ItemStatusId;
            item.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        private string GenerateSerialNumber()
        {
            var lastItem = _context.Items.OrderByDescending(i => i.ItemId).FirstOrDefault();
            int nextNumber = (lastItem?.ItemId ?? 0) + 1;
            return $"ITEM{nextNumber:D6}";
        }
    }
}
