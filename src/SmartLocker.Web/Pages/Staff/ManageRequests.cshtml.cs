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

        public ManageRequestsModel(RequestService requestService, LogService logService)
        {
            _requestService = requestService;
            _logService = logService;
        }

        public List<Request> UserRequests { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
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
                    ErrorMessage = "Request not found or you don't have permission to cancel it.";
                    return RedirectToPage();
                }

                if (request.RequestStatus?.RequestStatusName != "Pending")
                {
                    ErrorMessage = "Only pending requests can be cancelled.";
                    return RedirectToPage();
                }

                _requestService.RejectRequest(requestId, "Cancelled by user");
                _logService.LogAction(userId, "CancelRequest", "Request", requestId, $"User {username} cancelled request {requestId}", "Success");
                SuccessMessage = "Request cancelled successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error cancelling request: {ex.Message}";
                _logService.LogAction(userId, "CancelRequest", "Request", requestId, $"Failed to cancel request: {ex.Message}", "Failed");
            }

            return RedirectToPage();
        }
    }
}
