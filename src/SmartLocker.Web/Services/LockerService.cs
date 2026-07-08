using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Models;

namespace SmartLocker.Web.Services
{
    public class LockerService
    {
        private readonly SmartLockerDbContext _context;

        public LockerService(SmartLockerDbContext context)
        {
            _context = context;
        }

        public List<Locker> GetAllLockers()
        {
            return _context.Lockers
                .Include(l => l.LockerStatus)
                .Include(l => l.Items)
                .OrderBy(l => l.LockerName)
                .ToList();
        }

        public Locker GetLockerById(int lockerId)
        {
            return _context.Lockers
                .Include(l => l.LockerStatus)
                .Include(l => l.Items)
                .FirstOrDefault(l => l.LockerId == lockerId);
        }

        public List<Locker> GetAvailableLockers()
        {
            var availableStatus = _context.LockerStatuses.FirstOrDefault(s => s.LockerStatusName == "Available");
            if (availableStatus == null)
            {
                return new List<Locker>();
            }

            return _context.Lockers
                .Include(l => l.LockerStatus)
                .Include(l => l.Items)
                .Where(l => l.LockerStatusId == availableStatus.LockerStatusId && l.IsActive)
                .OrderBy(l => l.LockerName)
                .ToList();
        }

        public void UpdateLockerStatus(int lockerId, string statusName)
        {
            var locker = GetLockerById(lockerId);
            if (locker == null)
            {
                throw new Exception("Locker not found");
            }

            var status = _context.LockerStatuses.FirstOrDefault(s => s.LockerStatusName == statusName);
            if (status == null)
            {
                throw new Exception("Status not found");
            }

            locker.LockerStatusId = status.LockerStatusId;
            locker.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public LockerAccessToken GenerateAccessToken(int borrowId, int lockerId, int? createdByUserId = null, string purpose = "BorrowUnlock")
        {
            var borrow = _context.Borrows.FirstOrDefault(b => b.BorrowId == borrowId);
            if (borrow == null)
            {
                throw new Exception("Borrow not found");
            }

            var token = GenerateSecureToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(15); // Token expires in 15 minutes

            var accessToken = new LockerAccessToken
            {
                BorrowId = borrowId,
                UserId = borrow.UserId,
                ItemId = borrow.ItemId,
                LockerId = lockerId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                IsUsed = false,
                IsValid = true,
                Purpose = purpose,
                CreatedByUserId = createdByUserId,
                FailedAttemptCount = 0
            };

            _context.LockerAccessTokens.Add(accessToken);
            _context.SaveChanges();

            return accessToken;
        }

        public LockerAccessToken ValidateAccessToken(string token)
        {
            var accessToken = _context.LockerAccessTokens
                .Include(t => t.Borrow)
                .Include(t => t.Locker)
                .FirstOrDefault(t => t.Token == token);

            if (accessToken == null)
            {
                return null;
            }

            // Check if token is expired
            if (accessToken.ExpiresAt < DateTime.UtcNow)
            {
                accessToken.IsValid = false;
                _context.SaveChanges();
                return null;
            }

            // Check if token is already used
            if (accessToken.IsUsed)
            {
                return null;
            }

            return accessToken;
        }

        public void MarkTokenAsUsed(int tokenId)
        {
            var token = _context.LockerAccessTokens.FirstOrDefault(t => t.TokenId == tokenId);
            if (token == null)
            {
                throw new Exception("Token not found");
            }

            token.IsUsed = true;
            token.UsedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void UnlockLocker(int lockerId)
        {
            // This would call the hardware service in a real implementation
            // For now, just log the action
            var locker = GetLockerById(lockerId);
            if (locker == null)
            {
                throw new Exception("Locker not found");
            }

            // Update locker status to "Unlocked" (temporary state)
            UpdateLockerStatus(lockerId, "Available");
        }

        public void LockLocker(int lockerId)
        {
            // This would call the hardware service in a real implementation
            var locker = GetLockerById(lockerId);
            if (locker == null)
            {
                throw new Exception("Locker not found");
            }

            // Update locker status back to "Available"
            UpdateLockerStatus(lockerId, "Available");
        }

        private string GenerateSecureToken()
        {
            // Use cryptographically secure random token generation
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData).Replace("+", "-").Replace("/", "_").TrimEnd('=');
            }
        }

        private string GenerateRandomToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var token = new string(Enumerable.Range(0, 32)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());

            return token;
        }
    }
}
