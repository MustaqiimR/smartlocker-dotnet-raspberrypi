using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class RequestsModel : PageModel
    {
        private readonly RequestService _requestService;
        private readonly BorrowService _borrowService;
        private readonly LockerService _lockerService;
        private readonly LogService _logService;
        private readonly SmartLocker.Web.Data.SmartLockerDbContext _context;

        public List<Request> PendingRequests { get; set; } = new();
        public List<SmartLocker.Web.Models.Locker> AvailableLockers { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public RequestsModel(RequestService requestService, BorrowService borrowService, LockerService lockerService, LogService logService, SmartLocker.Web.Data.SmartLockerDbContext context)
        {
            _requestService = requestService;
            _borrowService = borrowService;
            _lockerService = lockerService;
            _logService = logService;
            _context = context;
        }

        public void OnGet()
        {
            PendingRequests = _requestService.GetPendingRequests();
            AvailableLockers = _lockerService.GetAvailableLockers();
        }

        public IActionResult OnPostApprove(int requestId, int lockerId, int borrowDays)
        {
            try
            {
                var request = _requestService.GetRequestById(requestId);
                if (request == null)
                {
                    ErrorMessage = "Request not found.";
                    return RedirectToPage();
                }

                // Approve the request
                _requestService.ApproveRequest(requestId);

                // Calculate due date
                var dueDate = DateTime.UtcNow.AddDays(borrowDays);

                // Create borrow record
                var borrow = _borrowService.CreateBorrow(request.UserId, request.ItemId, lockerId, requestId, dueDate);

                // Generate QR access token
                var accessToken = _lockerService.GenerateAccessToken(borrow.BorrowId, lockerId);

                // Log the action
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "ApproveRequest", "Request", requestId, $"Approved request {requestId}, created borrow {borrow.BorrowId} with token {accessToken.Token}", "Success");

                SuccessMessage = $"Request approved! Borrow record created. QR Token: {accessToken.Token}";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error approving request: {ex.Message}";
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "ApproveRequest", "Request", requestId, ex.Message, "Failed");
                return RedirectToPage();
            }
        }

        public IActionResult OnPostReject(int requestId, string rejectionReason)
        {
            try
            {
                var request = _requestService.GetRequestById(requestId);
                if (request == null)
                {
                    ErrorMessage = "Request not found.";
                    return RedirectToPage();
                }

                // Reject the request
                _requestService.RejectRequest(requestId, rejectionReason);

                // Log the action
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "RejectRequest", "Request", requestId, $"Rejected request {requestId}. Reason: {rejectionReason}", "Success");

                SuccessMessage = $"Request rejected.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error rejecting request: {ex.Message}";
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
                _logService.LogAction(int.Parse(currentUserId), "RejectRequest", "Request", requestId, ex.Message, "Failed");
                return RedirectToPage();
            }
        }
    }
}
