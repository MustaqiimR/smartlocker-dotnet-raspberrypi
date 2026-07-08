using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages
{
    public class UnlockModel : PageModel
    {
        private readonly LockerService _lockerService;
        private readonly BorrowService _borrowService;
        private readonly LogService _logService;
        private readonly ILockerHardwareService _hardwareService;

        public string Token { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public SmartLocker.Web.Models.LockerAccessToken AccessToken { get; set; }

        public UnlockModel(LockerService lockerService, BorrowService borrowService, LogService logService, ILockerHardwareService hardwareService)
        {
            _lockerService = lockerService;
            _borrowService = borrowService;
            _logService = logService;
            _hardwareService = hardwareService;
        }

        public async Task OnGetAsync(string token)
        {
            Token = token;
            IsSuccess = false;

            try
            {
                // Validate token format
                if (string.IsNullOrWhiteSpace(token))
                {
                    ErrorMessage = "Invalid token format.";
                    _logService.LogAction(0, "UnlockAttempt", "LockerAccessToken", 0, "Invalid token format", "Failed");
                    return;
                }

                // Validate token exists and is not expired/used
                AccessToken = _lockerService.ValidateAccessToken(token);
                if (AccessToken == null)
                {
                    ErrorMessage = "Token is invalid, expired, or already used.";
                    _logService.LogAction(0, "UnlockAttempt", "LockerAccessToken", 0, $"Invalid/expired/used token: {token}", "Failed");
                    
                    // Increment failed attempt count
                    var failedToken = await GetTokenByStringAsync(token);
                    if (failedToken != null)
                    {
                        failedToken.FailedAttemptCount++;
                        // Lock out token if too many failed attempts
                        if (failedToken.FailedAttemptCount >= 5)
                        {
                            failedToken.IsValid = false;
                        }
                    }
                    return;
                }

                // Validate borrow is still active
                var borrow = AccessToken.Borrow;
                if (borrow == null || borrow.BorrowStatus?.BorrowStatusName != "Active")
                {
                    ErrorMessage = "Borrow is no longer active.";
                    _logService.LogAction(AccessToken.UserId, "UnlockAttempt", "Borrow", borrow?.BorrowId ?? 0, "Borrow not active", "Failed");
                    return;
                }

                // Validate locker exists
                var locker = AccessToken.Locker;
                if (locker == null || !locker.IsActive)
                {
                    ErrorMessage = "Locker is not available.";
                    _logService.LogAction(AccessToken.UserId, "UnlockAttempt", "Locker", AccessToken.LockerId, "Locker not available", "Failed");
                    return;
                }

                // Call hardware service to unlock
                await _hardwareService.UnlockLockerAsync(AccessToken.LockerId);

                // Mark token as used
                _lockerService.MarkTokenAsUsed(AccessToken.TokenId);

                // Log successful unlock
                _logService.LogAction(AccessToken.UserId, "UnlockSuccess", "Locker", AccessToken.LockerId, $"Successfully unlocked locker {AccessToken.LockerId} for borrow {borrow.BorrowId}", "Success");

                IsSuccess = true;
                Message = $"Locker {locker.LockerName} unlocked successfully! Please retrieve your item.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred during unlock: {ex.Message}";
                _logService.LogAction(0, "UnlockAttempt", "LockerAccessToken", 0, $"Exception: {ex.Message}", "Failed");
            }
        }

        private async Task<SmartLocker.Web.Models.LockerAccessToken> GetTokenByStringAsync(string token)
        {
            // This is a placeholder - in a real implementation, you'd query the database
            // For now, we'll return null to avoid additional database queries
            return null;
        }
    }
}
