using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Locker
{
    public class UnlockModel : PageModel
    {
        private readonly LockerService _lockerService;
        private readonly LogService _logService;

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public UnlockModel(LockerService lockerService, LogService logService)
        {
            _lockerService = lockerService;
            _logService = logService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    ErrorMessage = "Token is required.";
                    return Page();
                }

                var accessToken = _lockerService.ValidateAccessToken(token);
                
                if (accessToken == null)
                {
                    ErrorMessage = "Invalid or expired token.";
                    _logService.LogAction(0, "UnlockAttempt", "LockerAccessToken", 0, $"Invalid token: {token}", "Failed");
                    return Page();
                }

                if (accessToken.IsUsed)
                {
                    ErrorMessage = "This token has already been used.";
                    _logService.LogAction(0, "UnlockAttempt", "LockerAccessToken", accessToken.TokenId, "Token already used", "Failed");
                    return Page();
                }

                if (accessToken.ExpiresAt < DateTime.UtcNow)
                {
                    ErrorMessage = "This token has expired.";
                    _logService.LogAction(0, "UnlockAttempt", "LockerAccessToken", accessToken.TokenId, "Token expired", "Failed");
                    return Page();
                }

                // Mark token as used
                _lockerService.MarkTokenAsUsed(accessToken.TokenId);

                // Trigger locker unlock (mock or GPIO)
                _lockerService.UnlockLocker(accessToken.LockerId);

                // Log unlock success and item taken
                _logService.LogAction(accessToken.UserId, "UnlockSuccess", "Locker", accessToken.LockerId, $"Locker {accessToken.LockerId} unlocked", "Success");
                _logService.LogAction(accessToken.UserId, "ItemTaken", "Item", accessToken.ItemId, $"Item {accessToken.ItemId} taken from locker {accessToken.LockerId}", "Success");

                SuccessMessage = $"Locker unlocked successfully! Please retrieve your item from Locker {accessToken.Locker?.LockerName}.";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                _logService.LogAction(0, "UnlockError", "Locker", 0, ex.Message, "Failed");
                return Page();
            }
        }
    }
}
