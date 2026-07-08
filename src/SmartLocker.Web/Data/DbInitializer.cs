using SmartLocker.Web.Models;
using System.Security.Cryptography;
using System.Text;

namespace SmartLocker.Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SmartLockerDbContext context)
        {
            // Check if database already has data
            if (context.Roles.Any())
            {
                return; // Database has been seeded
            }

            // Create roles
            var adminRole = new Role
            {
                RoleName = "Admin",
                Description = "Administrator with full system access"
            };

            var staffRole = new Role
            {
                RoleName = "Staff",
                Description = "Staff member with access to borrow/return operations"
            };

            context.Roles.AddRange(adminRole, staffRole);
            context.SaveChanges();

            // Create users
            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = HashPassword("admin123"),
                FullName = "System Administrator",
                Email = "admin@smartlocker.local",
                UserRegNo = "ADM001",
                RoleId = adminRole.RoleId,
                IsActive = true
            };

            var staffUser = new User
            {
                Username = "staff",
                PasswordHash = HashPassword("staff123"),
                FullName = "Staff Member",
                Email = "staff@smartlocker.local",
                UserRegNo = "STF001",
                RoleId = staffRole.RoleId,
                IsActive = true
            };

            context.Users.AddRange(adminUser, staffUser);
            context.SaveChanges();

            // Create item statuses
            var statusAvailable = new ItemStatus
            {
                ItemStatusName = "Available",
                Description = "Item is available for borrowing"
            };

            var statusBorrowed = new ItemStatus
            {
                ItemStatusName = "Borrowed",
                Description = "Item is currently borrowed"
            };

            var statusMaintenance = new ItemStatus
            {
                ItemStatusName = "Maintenance",
                Description = "Item is under maintenance"
            };

            var statusLost = new ItemStatus
            {
                ItemStatusName = "Lost",
                Description = "Item is lost"
            };

            context.ItemStatuses.AddRange(statusAvailable, statusBorrowed, statusMaintenance, statusLost);
            context.SaveChanges();

            // Create locker statuses
            var lockerAvailable = new LockerStatus
            {
                LockerStatusName = "Available",
                Description = "Locker is available"
            };

            var lockerOccupied = new LockerStatus
            {
                LockerStatusName = "Occupied",
                Description = "Locker is occupied with an item"
            };

            var lockerLocked = new LockerStatus
            {
                LockerStatusName = "Locked",
                Description = "Locker is locked"
            };

            var lockerMaintenance = new LockerStatus
            {
                LockerStatusName = "Maintenance",
                Description = "Locker is under maintenance"
            };

            context.LockerStatuses.AddRange(lockerAvailable, lockerOccupied, lockerLocked, lockerMaintenance);
            context.SaveChanges();

            // Create request statuses
            var requestPending = new RequestStatus
            {
                RequestStatusName = "Pending",
                Description = "Request is pending approval"
            };

            var requestApproved = new RequestStatus
            {
                RequestStatusName = "Approved",
                Description = "Request has been approved"
            };

            var requestRejected = new RequestStatus
            {
                RequestStatusName = "Rejected",
                Description = "Request has been rejected"
            };

            context.RequestStatuses.AddRange(requestPending, requestApproved, requestRejected);
            context.SaveChanges();

            // Create borrow statuses
            var borrowActive = new BorrowStatus
            {
                BorrowStatusName = "Active",
                Description = "Borrow is active"
            };

            var borrowReturned = new BorrowStatus
            {
                BorrowStatusName = "Returned",
                Description = "Item has been returned"
            };

            var borrowOverdue = new BorrowStatus
            {
                BorrowStatusName = "Overdue",
                Description = "Borrow is overdue"
            };

            var borrowLost = new BorrowStatus
            {
                BorrowStatusName = "Lost",
                Description = "Item is lost"
            };

            context.BorrowStatuses.AddRange(borrowActive, borrowReturned, borrowOverdue, borrowLost);
            context.SaveChanges();

            // Create categories
            var categoryTools = new Category
            {
                CategoryName = "Tools",
                Description = "Tools and equipment"
            };

            var categoryElectronics = new Category
            {
                CategoryName = "Electronics",
                Description = "Electronic devices and components"
            };

            var categoryDocuments = new Category
            {
                CategoryName = "Documents",
                Description = "Important documents and files"
            };

            context.Categories.AddRange(categoryTools, categoryElectronics, categoryDocuments);
            context.SaveChanges();

            // Create lockers
            var locker1 = new Locker
            {
                LockerName = "Locker-01",
                Location = "Main Hall",
                LockerStatusId = lockerAvailable.LockerStatusId,
                GpioPin = "17",
                IsActive = true
            };

            var locker2 = new Locker
            {
                LockerName = "Locker-02",
                Location = "Main Hall",
                LockerStatusId = lockerAvailable.LockerStatusId,
                GpioPin = "27",
                IsActive = true
            };

            var locker3 = new Locker
            {
                LockerName = "Locker-03",
                Location = "Storage Room",
                LockerStatusId = lockerAvailable.LockerStatusId,
                GpioPin = "22",
                IsActive = true
            };

            context.Lockers.AddRange(locker1, locker2, locker3);
            context.SaveChanges();

            // Create sample items
            var item1 = new Item
            {
                ItemName = "Laptop",
                Description = "Dell Laptop for development",
                CategoryId = categoryElectronics.CategoryId,
                ItemStatusId = statusAvailable.ItemStatusId,
                LockerId = locker1.LockerId,
                SerialNumber = "DELL-001"
            };

            var item2 = new Item
            {
                ItemName = "Power Drill",
                Description = "Cordless power drill",
                CategoryId = categoryTools.CategoryId,
                ItemStatusId = statusAvailable.ItemStatusId,
                LockerId = locker2.LockerId,
                SerialNumber = "DRILL-001"
            };

            var item3 = new Item
            {
                ItemName = "Project Files",
                Description = "Important project documentation",
                CategoryId = categoryDocuments.CategoryId,
                ItemStatusId = statusAvailable.ItemStatusId,
                LockerId = locker3.LockerId,
                SerialNumber = "DOC-001"
            };

            context.Items.AddRange(item1, item2, item3);
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
