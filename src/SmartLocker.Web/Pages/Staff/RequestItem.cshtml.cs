using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLocker.Web.Models;
using SmartLocker.Web.Services;

namespace SmartLocker.Web.Pages.Staff
{
    [Authorize(Roles = "Staff,Admin")]
    public class RequestItemModel : PageModel
    {
        private readonly ItemService _itemService;
        private readonly RequestService _requestService;
        private readonly LogService _logService;

        public Item Item { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public RequestItemModel(ItemService itemService, RequestService requestService, LogService logService)
        {
            _itemService = itemService;
            _requestService = requestService;
            _logService = logService;
        }

        public void OnGet(int id)
        {
            Item = _itemService.GetItemById(id);
        }

        public IActionResult OnPost(int itemId, string justification)
        {
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            try
            {
                if (string.IsNullOrEmpty(justification))
                {
                    ErrorMessage = "Justification is required.";
                    Item = _itemService.GetItemById(itemId);
                    return Page();
                }

                var request = _requestService.CreateRequest(currentUserId, itemId, justification);
                _logService.LogAction(currentUserId, "Create", "Request", request.RequestId, $"Created request for item {itemId}", "Success");
                
                SuccessMessage = "Request submitted successfully. Please wait for approval.";
                Item = _itemService.GetItemById(itemId);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                _logService.LogAction(currentUserId, "Create", "Request", itemId, ex.Message, "Failed");
                Item = _itemService.GetItemById(itemId);
            }

            return Page();
        }
    }
}
