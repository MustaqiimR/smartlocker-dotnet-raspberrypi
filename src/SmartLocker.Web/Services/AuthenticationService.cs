using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Models;
using System.Security.Cryptography;
using System.Text;

namespace SmartLocker.Web.Services
{
    public class AuthenticationService
    {
        private readonly SmartLockerDbContext _context;

        public AuthenticationService(SmartLockerDbContext context)
        {
            _context = context;
        }

        public User AuthenticateUser(string username, string password)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == username && u.IsActive);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            _context.SaveChanges();

            return user;
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public User GetUserById(int userId)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserId == userId);
        }
    }
}
