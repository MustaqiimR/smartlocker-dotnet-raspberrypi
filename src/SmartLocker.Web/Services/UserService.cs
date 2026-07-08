using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Models;

namespace SmartLocker.Web.Services
{
    public class UserService
    {
        private readonly SmartLockerDbContext _context;
        private readonly AuthenticationService _authService;

        public UserService(SmartLockerDbContext context, AuthenticationService authService)
        {
            _context = context;
            _authService = authService;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users
                .Include(u => u.Role)
                .OrderBy(u => u.Username)
                .ToList();
        }

        public User GetUserById(int userId)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserId == userId);
        }

        public User CreateUser(string username, string password, string fullName, string email, int roleId)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                throw new Exception("Username already exists");
            }

            var user = new User
            {
                Username = username,
                PasswordHash = _authService.HashPassword(password),
                FullName = fullName,
                Email = email,
                RoleId = roleId,
                IsActive = true,
                UserRegNo = GenerateUserRegNo()
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void UpdateUser(int userId, string fullName, string email, int roleId, bool isActive)
        {
            var user = GetUserById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FullName = fullName;
            user.Email = email;
            user.RoleId = roleId;
            user.IsActive = isActive;
            user.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void ChangePassword(int userId, string newPassword)
        {
            var user = GetUserById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.PasswordHash = _authService.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void DisableUser(int userId)
        {
            var user = GetUserById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        private string GenerateUserRegNo()
        {
            var lastUser = _context.Users.OrderByDescending(u => u.UserId).FirstOrDefault();
            int nextNumber = (lastUser?.UserId ?? 0) + 1;
            return $"USR{nextNumber:D4}";
        }
    }
}
