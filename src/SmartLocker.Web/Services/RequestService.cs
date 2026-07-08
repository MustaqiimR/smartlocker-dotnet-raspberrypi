using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Models;

namespace SmartLocker.Web.Services
{
    public class RequestService
    {
        private readonly SmartLockerDbContext _context;

        public RequestService(SmartLockerDbContext context)
        {
            _context = context;
        }

        public List<Request> GetAllRequests()
        {
            return _context.Requests
                .Include(r => r.User)
                .Include(r => r.Item)
                .Include(r => r.RequestStatus)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public Request GetRequestById(int requestId)
        {
            return _context.Requests
                .Include(r => r.User)
                .Include(r => r.Item)
                .Include(r => r.RequestStatus)
                .FirstOrDefault(r => r.RequestId == requestId);
        }

        public List<Request> GetPendingRequests()
        {
            var pendingStatus = _context.RequestStatuses.FirstOrDefault(s => s.RequestStatusName == "Pending");
            if (pendingStatus == null)
            {
                return new List<Request>();
            }

            return _context.Requests
                .Include(r => r.User)
                .Include(r => r.Item)
                .Include(r => r.RequestStatus)
                .Where(r => r.RequestStatusId == pendingStatus.RequestStatusId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public List<Request> GetUserRequests(int userId)
        {
            return _context.Requests
                .Include(r => r.User)
                .Include(r => r.Item)
                .Include(r => r.RequestStatus)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public Request CreateRequest(int userId, int itemId, string justification)
        {
            // Validate item exists and is available
            var item = _context.Items
                .Include(i => i.ItemStatus)
                .FirstOrDefault(i => i.ItemId == itemId);
            
            if (item == null)
            {
                throw new Exception("Item not found");
            }

            var availableStatus = _context.ItemStatuses.FirstOrDefault(s => s.ItemStatusName == "Available");
            if (item.ItemStatusId != availableStatus?.ItemStatusId)
            {
                throw new Exception("Item is not available for borrowing");
            }

            // Check for duplicate pending request
            var pendingStatus = _context.RequestStatuses.FirstOrDefault(s => s.RequestStatusName == "Pending");
            var existingRequest = _context.Requests
                .FirstOrDefault(r => r.UserId == userId && 
                                    r.ItemId == itemId && 
                                    r.RequestStatusId == pendingStatus.RequestStatusId);
            
            if (existingRequest != null)
            {
                throw new Exception("You already have a pending request for this item");
            }

            if (pendingStatus == null)
            {
                throw new Exception("Pending status not found");
            }

            var request = new Request
            {
                UserId = userId,
                ItemId = itemId,
                RequestStatusId = pendingStatus.RequestStatusId,
                Justification = justification,
                CreatedAt = DateTime.UtcNow
            };

            _context.Requests.Add(request);
            _context.SaveChanges();

            return request;
        }

        public void ApproveRequest(int requestId)
        {
            var request = GetRequestById(requestId);
            if (request == null)
            {
                throw new Exception("Request not found");
            }

            // Validate request is still pending
            var pendingStatus = _context.RequestStatuses.FirstOrDefault(s => s.RequestStatusName == "Pending");
            if (request.RequestStatusId != pendingStatus?.RequestStatusId)
            {
                throw new Exception("Request is not in pending status");
            }

            // Validate item is still available
            var item = _context.Items.Include(i => i.ItemStatus).FirstOrDefault(i => i.ItemId == request.ItemId);
            var availableStatus = _context.ItemStatuses.FirstOrDefault(s => s.ItemStatusName == "Available");
            if (item?.ItemStatusId != availableStatus?.ItemStatusId)
            {
                throw new Exception("Item is no longer available");
            }

            var approvedStatus = _context.RequestStatuses.FirstOrDefault(s => s.RequestStatusName == "Approved");
            if (approvedStatus == null)
            {
                throw new Exception("Approved status not found");
            }

            request.RequestStatusId = approvedStatus.RequestStatusId;
            request.ApprovedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void RejectRequest(int requestId, string rejectionReason)
        {
            var request = GetRequestById(requestId);
            if (request == null)
            {
                throw new Exception("Request not found");
            }

            // Validate request is still pending
            var pendingStatus = _context.RequestStatuses.FirstOrDefault(s => s.RequestStatusName == "Pending");
            if (request.RequestStatusId != pendingStatus?.RequestStatusId)
            {
                throw new Exception("Request is not in pending status");
            }

            var rejectedStatus = _context.RequestStatuses.FirstOrDefault(s => s.RequestStatusName == "Rejected");
            if (rejectedStatus == null)
            {
                throw new Exception("Rejected status not found");
            }

            request.RequestStatusId = rejectedStatus.RequestStatusId;
            request.RejectionReason = rejectionReason ?? "No reason provided";
            request.RejectedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }
    }
}
