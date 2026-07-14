using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Models;

namespace SmartLocker.Web.Services
{
    public class BorrowLogService
    {
        private readonly SmartLockerDbContext _context;

        public BorrowLogService(SmartLockerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Log a borrow-related action
        /// </summary>
        public async Task LogBorrowActionAsync(
            int itemId,
            int userId,
            string action,
            string actionDetails = null,
            string previousStatus = null,
            string newStatus = null,
            string notes = null,
            int? borrowId = null)
        {
            var log = new BorrowLog
            {
                BorrowId = borrowId,
                ItemId = itemId,
                UserId = userId,
                Action = action,
                ActionDetails = actionDetails,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                Notes = notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.BorrowLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get all borrow logs for an item, ordered by most recent
        /// </summary>
        public async Task<List<BorrowLog>> GetItemBorrowHistoryAsync(int itemId)
        {
            return await _context.BorrowLogs
                .Where(bl => bl.ItemId == itemId)
                .OrderByDescending(bl => bl.CreatedAt)
                .Include(bl => bl.User)
                .Include(bl => bl.Borrow)
                .ToListAsync();
        }

        /// <summary>
        /// Get the most recent borrow for an item
        /// </summary>
        public async Task<BorrowLog> GetMostRecentBorrowAsync(int itemId)
        {
            return await _context.BorrowLogs
                .Where(bl => bl.ItemId == itemId && (bl.Action == "Borrowed" || bl.Action == "Returned"))
                .OrderByDescending(bl => bl.CreatedAt)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get borrow logs for a specific borrow record
        /// </summary>
        public async Task<List<BorrowLog>> GetBorrowLogsAsync(int borrowId)
        {
            return await _context.BorrowLogs
                .Where(bl => bl.BorrowId == borrowId)
                .OrderByDescending(bl => bl.CreatedAt)
                .Include(bl => bl.User)
                .ToListAsync();
        }

        /// <summary>
        /// Get all borrow logs for a user
        /// </summary>
        public async Task<List<BorrowLog>> GetUserBorrowLogsAsync(int userId)
        {
            return await _context.BorrowLogs
                .Where(bl => bl.UserId == userId)
                .OrderByDescending(bl => bl.CreatedAt)
                .Include(bl => bl.Item)
                .Include(bl => bl.User)
                .ToListAsync();
        }

        /// <summary>
        /// Get recent borrow activity (last N days)
        /// </summary>
        public async Task<List<BorrowLog>> GetRecentActivityAsync(int daysBack = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysBack);
            return await _context.BorrowLogs
                .Where(bl => bl.CreatedAt >= cutoffDate)
                .OrderByDescending(bl => bl.CreatedAt)
                .Include(bl => bl.Item)
                .Include(bl => bl.User)
                .ToListAsync();
        }

        /// <summary>
        /// Get borrow statistics for an item
        /// </summary>
        public async Task<BorrowItemStatsDto> GetItemBorrowStatsAsync(int itemId)
        {
            var logs = await GetItemBorrowHistoryAsync(itemId);
            var borrowCount = logs.Count(l => l.Action == "Borrowed");
            var returnCount = logs.Count(l => l.Action == "Returned");
            var lastBorrow = logs.FirstOrDefault(l => l.Action == "Borrowed" || l.Action == "Returned");

            return new BorrowItemStatsDto
            {
                ItemId = itemId,
                TotalBorrows = borrowCount,
                TotalReturns = returnCount,
                LastBorrowDate = lastBorrow?.CreatedAt,
                LastBorrowedBy = lastBorrow?.User?.FullName,
                TotalLogEntries = logs.Count
            };
        }
    }

    public class BorrowItemStatsDto
    {
        public int ItemId { get; set; }
        public int TotalBorrows { get; set; }
        public int TotalReturns { get; set; }
        public DateTime? LastBorrowDate { get; set; }
        public string LastBorrowedBy { get; set; }
        public int TotalLogEntries { get; set; }
    }
}
