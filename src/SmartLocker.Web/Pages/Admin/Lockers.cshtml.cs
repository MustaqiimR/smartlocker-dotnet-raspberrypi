using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class LockersModel : PageModel
    {
        private readonly SmartLocker.Web.Data.SmartLockerDbContext _context;
        private readonly LogService _logService;

        public List<Models.Locker> Lockers { get; set; } = new();
        public Models.Locker EditingLocker { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public LockersModel(SmartLocker.Web.Data.SmartLockerDbContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public void OnGet(int? editId)
        {
            if (editId.HasValue)
            {
                EditingLocker = _context.Lockers
                    .FirstOrDefault(l => l.LockerId == editId.Value);
            }
            LoadData();
        }

        public IActionResult OnPost(string action, int lockerId, string lockerName, int statusId)
        {
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            try
            {
                if (action == "create")
                {
                    if (string.IsNullOrEmpty(lockerName))
                    {
                        ErrorMessage = "Locker name is required.";
                    }
                    else if (_context.Lockers.Any(l => l.LockerName == lockerName))
                    {
                        ErrorMessage = "Locker name already exists.";
                    }
                    else
                    {
                        var locker = new Models.Locker
                        {
                            LockerName = lockerName,
                            LockerStatusId = 1, // Available
                            CreatedAt = DateTime.UtcNow,
                            IsActive = true
                        };
                        _context.Lockers.Add(locker);
                        _context.SaveChanges();
                        _logService.LogAction(currentUserId, "Create", "Locker", locker.LockerId, $"Created locker: {lockerName}", "Success");
                        SuccessMessage = $"Locker '{lockerName}' created successfully.";
                    }
                }
                else if (action == "edit")
                {
                    var locker = _context.Lockers.FirstOrDefault(l => l.LockerId == lockerId);
                    if (locker != null)
                    {
                        if (!string.IsNullOrEmpty(lockerName))
                        {
                            locker.LockerName = lockerName;
                        }
                        if (statusId > 0)
                        {
                            locker.LockerStatusId = statusId;
                        }
                        locker.UpdatedAt = DateTime.UtcNow;
                        _context.Lockers.Update(locker);
                        _context.SaveChanges();
                        _logService.LogAction(currentUserId, "Edit", "Locker", lockerId, $"Updated locker: {locker.LockerName}", "Success");
                        SuccessMessage = $"Locker '{locker.LockerName}' updated successfully.";
                        EditingLocker = null;
                    }
                    else
                    {
                        ErrorMessage = "Locker not found.";
                    }
                }
                else if (action == "delete")
                {
                    var locker = _context.Lockers.FirstOrDefault(l => l.LockerId == lockerId);
                    if (locker != null)
                    {
                        try
                        {
                            _context.Lockers.Remove(locker);
                            _context.SaveChanges();
                            _logService.LogAction(currentUserId, "Delete", "Locker", lockerId, $"Deleted locker: {locker.LockerName}", "Success");
                            SuccessMessage = $"Locker '{locker.LockerName}' deleted successfully.";
                        }
                        catch (Exception deleteEx)
                        {
                            ErrorMessage = $"Cannot delete locker: {deleteEx.InnerException?.Message ?? deleteEx.Message}";
                            _logService.LogAction(currentUserId, "Delete", "Locker", lockerId, deleteEx.Message, "Failed");
                        }
                    }
                    else
                    {
                        ErrorMessage = "Locker not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                _logService.LogAction(currentUserId, action, "Locker", lockerId, ex.Message, "Failed");
            }

            LoadData();
            return Page();
        }

        private void LoadData()
        {
            Lockers = _context.Lockers
                .OrderBy(l => l.LockerName)
                .ToList();
        }
    }
}
