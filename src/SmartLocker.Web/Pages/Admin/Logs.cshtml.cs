using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class LogsModel : PageModel
    {
        private readonly LogService _logService;

        public List<SystemLog> Logs { get; set; } = new();

        public LogsModel(LogService logService)
        {
            _logService = logService;
        }

        public void OnGet(string action, string resourceType, DateTime? startDate, DateTime? endDate)
        {
            var allLogs = _logService.GetAllLogs();

            if (!string.IsNullOrEmpty(action))
            {
                allLogs = allLogs.Where(l => l.Action.Contains(action, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(resourceType))
            {
                allLogs = allLogs.Where(l => l.ResourceType.Contains(resourceType, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (startDate.HasValue)
            {
                allLogs = allLogs.Where(l => l.CreatedAt >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                allLogs = allLogs.Where(l => l.CreatedAt <= endDate.Value.AddDays(1)).ToList();
            }

            Logs = allLogs.Take(1000).ToList(); // Limit to 1000 most recent logs
        }
    }
}
