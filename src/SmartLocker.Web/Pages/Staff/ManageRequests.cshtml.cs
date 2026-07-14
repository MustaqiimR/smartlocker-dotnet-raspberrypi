using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Staff
{
    [Authorize(Roles = "Staff,Admin")]
    public class ManageRequestsModel : PageModel
    {
        private readonly RequestService _requestService;
        private readonly LogService _logService;

        public List<Request> UserRequests { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public ManageRequestsModel(RequestService requestService, LogService logService)
        {
            _requestService = requestService;
            _logService = logService;
        }

        public void OnGet()
        {
            SuccessMessage = TempData["SuccessMessage"] as string;
            ErrorMessage = TempData["ErrorMessage"] as string;
            
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            UserRequests = _requestService.GetUserRequests(userId);
        }

        public IActionResult OnPostCancel(int requestId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

            try
            {
                var request = _requestService.GetRequestById(requestId);
                if (request == null || request.UserId != userId)
                {
                    TempData["ErrorMessage"] = "Request not found or you don't have permission to cancel it.";
                    return RedirectToPage();
                }

                if (request.RequestStatus?.RequestStatusName != "Pending")
                {
                    TempData["ErrorMessage"] = "Only pending requests can be cancelled.";
                    return RedirectToPage();
                }

                _requestService.RejectRequest(requestId, "Cancelled by user");
                _logService.LogAction(userId, "CancelRequest", "Request", requestId, $"User {username} cancelled request {requestId}", "Success");
                TempData["SuccessMessage"] = "Request cancelled successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error cancelling request: {ex.Message}";
                _logService.LogAction(userId, "CancelRequest", "Request", requestId, $"Failed to cancel request: {ex.Message}", "Failed");
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int requestId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

            try
            {
                var request = _requestService.GetRequestById(requestId);
                if (request == null || request.UserId != userId)
                {
                    TempData["ErrorMessage"] = "Request not found or you don't have permission to delete it.";
                    return RedirectToPage();
                }

                _requestService.DeleteRequest(requestId);
                _logService.LogAction(userId, "DeleteRequest", "Request", requestId, $"User {username} deleted request {requestId}", "Success");
                TempData["SuccessMessage"] = "Request deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting request: {ex.Message}";
                _logService.LogAction(userId, "DeleteRequest", "Request", requestId, $"Failed to delete request: {ex.Message}", "Failed");
            }

            return RedirectToPage();
        }
    }
}
