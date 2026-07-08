using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Models;

namespace SmartLocker.Web.Services
{
    public class LogService
    {
        private readonly SmartLockerDbContext _context;

        public LogService(SmartLockerDbContext context)
        {
            _context = context;
        }

        public List<SystemLog> GetAllLogs()
        {
            return _context.SystemLogs
                .Include(l => l.User)
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }

        public List<SystemLog> GetLogsByUser(int userId)
        {
            return _context.SystemLogs
                .Include(l => l.User)
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }

        public List<SystemLog> GetLogsByAction(string action)
        {
            return _context.SystemLogs
                .Include(l => l.User)
                .Where(l => l.Action == action)
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }

        public List<SystemLog> GetLogsByResourceType(string resourceType)
        {
            return _context.SystemLogs
                .Include(l => l.User)
                .Where(l => l.ResourceType == resourceType)
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }

        public void LogAction(int? userId, string action, string resourceType, int? resourceId, string description, string status = "Success")
        {
            var log = new SystemLog
            {
                UserId = userId,
                Action = action,
                ResourceType = resourceType,
                ResourceId = resourceId,
                Description = description,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            _context.SystemLogs.Add(log);
            _context.SaveChanges();
        }

        public List<SystemLog> GetLogsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.SystemLogs
                .Include(l => l.User)
                .Where(l => l.CreatedAt >= startDate && l.CreatedAt <= endDate)
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }
    }
}
