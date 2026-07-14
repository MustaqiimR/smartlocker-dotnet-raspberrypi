using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Models;

namespace SmartLocker.Web.Data
{
    public class SmartLockerDbContext : DbContext
    {
        public SmartLockerDbContext(DbContextOptions<SmartLockerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ItemStatus> ItemStatuses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<LockerStatus> LockerStatuses { get; set; }
        public DbSet<Locker> Lockers { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<BorrowStatus> BorrowStatuses { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<LockerAccessToken> LockerAccessTokens { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ItemStatus)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.ItemStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Locker)
                .WithMany(l => l.Items)
                .HasForeignKey(i => i.LockerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Locker>()
                .HasOne(l => l.LockerStatus)
                .WithMany(s => s.Lockers)
                .HasForeignKey(l => l.LockerStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Item)
                .WithMany(i => i.Requests)
                .HasForeignKey(r => r.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.RequestStatus)
                .WithMany(s => s.Requests)
                .HasForeignKey(r => r.RequestStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.User)
                .WithMany(u => u.Borrows)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.Item)
                .WithMany(i => i.Borrows)
                .HasForeignKey(b => b.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.Locker)
                .WithMany(l => l.Borrows)
                .HasForeignKey(b => b.LockerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.BorrowStatus)
                .WithMany(s => s.Borrows)
                .HasForeignKey(b => b.BorrowStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.Request)
                .WithOne(r => r.Borrow)
                .HasForeignKey<Borrow>(b => b.RequestId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<LockerAccessToken>()
                .HasOne(t => t.Borrow)
                .WithMany(b => b.AccessTokens)
                .HasForeignKey(t => t.BorrowId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LockerAccessToken>()
                .HasOne(t => t.Locker)
                .WithMany(l => l.AccessTokens)
                .HasForeignKey(t => t.LockerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SystemLog>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Create indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Locker>()
                .HasIndex(l => l.LockerName)
                .IsUnique();

            modelBuilder.Entity<Item>()
                .HasIndex(i => i.SerialNumber)
                .IsUnique();

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Administrator" },
                new Role { RoleId = 2, RoleName = "Staff", Description = "Staff Member" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    UserId = 1, 
                    Username = "admin", 
                    Email = "admin@smartlocker.local",
                    UserRegNo = "ADM001",
                    FullName = "System Administrator",
                    PasswordHash = "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcg7b3XeKeUxWdeS86E36P4/OFS",
                    IsActive = true,
                    RoleId = 1,
                    CreatedAt = DateTime.Now
                },
                new User 
                { 
                    UserId = 2, 
                    Username = "staff1", 
                    Email = "staff1@smartlocker.local",
                    UserRegNo = "STF001",
                    FullName = "John Doe",
                    PasswordHash = "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcg7b3XeKeUxWdeS86E36P4/OFS",
                    IsActive = true,
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Electronics", Description = "Electronic devices" },
                new Category { CategoryId = 2, CategoryName = "Tools", Description = "Tools and equipment" },
                new Category { CategoryId = 3, CategoryName = "Documents", Description = "Important documents" }
            );

            // Seed Item Statuses
            modelBuilder.Entity<ItemStatus>().HasData(
                new ItemStatus { ItemStatusId = 1, ItemStatusName = "Available", Description = "Available for borrowing" },
                new ItemStatus { ItemStatusId = 2, ItemStatusName = "Borrowed", Description = "Currently borrowed" },
                new ItemStatus { ItemStatusId = 3, ItemStatusName = "Maintenance", Description = "Under maintenance" },
                new ItemStatus { ItemStatusId = 4, ItemStatusName = "Lost", Description = "Item lost" }
            );

            // Seed Locker Statuses
            modelBuilder.Entity<LockerStatus>().HasData(
                new LockerStatus { LockerStatusId = 1, LockerStatusName = "Available", Description = "Locker available" },
                new LockerStatus { LockerStatusId = 2, LockerStatusName = "Occupied", Description = "Locker occupied" },
                new LockerStatus { LockerStatusId = 3, LockerStatusName = "Locked", Description = "Locker locked" },
                new LockerStatus { LockerStatusId = 4, LockerStatusName = "Maintenance", Description = "Locker under maintenance" }
            );

            // Seed Lockers
            modelBuilder.Entity<Locker>().HasData(
                new Locker { LockerId = 1, LockerName = "Locker-01", Location = "Floor 1", LockerStatusId = 1, GpioPin = "GPIO17", CreatedAt = DateTime.Now },
                new Locker { LockerId = 2, LockerName = "Locker-02", Location = "Floor 1", LockerStatusId = 1, GpioPin = "GPIO27", CreatedAt = DateTime.Now },
                new Locker { LockerId = 3, LockerName = "Locker-03", Location = "Floor 2", LockerStatusId = 1, GpioPin = "GPIO22", CreatedAt = DateTime.Now }
            );

            // Seed Items
            modelBuilder.Entity<Item>().HasData(
                new Item 
                { 
                    ItemId = 1, 
                    ItemName = "Laptop", 
                    SerialNumber = "LAP001",
                    Description = "Dell Laptop",
                    CategoryId = 1,
                    ItemStatusId = 1,
                    LockerId = 1,
                    CreatedAt = DateTime.Now
                },
                new Item 
                { 
                    ItemId = 2, 
                    ItemName = "Projector", 
                    SerialNumber = "PROJ001",
                    Description = "HD Projector",
                    CategoryId = 1,
                    ItemStatusId = 1,
                    LockerId = 2,
                    CreatedAt = DateTime.Now
                },
                new Item 
                { 
                    ItemId = 3, 
                    ItemName = "Power Drill", 
                    SerialNumber = "DRILL001",
                    Description = "Cordless Power Drill",
                    CategoryId = 2,
                    ItemStatusId = 1,
                    LockerId = 3,
                    CreatedAt = DateTime.Now
                }
            );

            // Seed Request Statuses
            modelBuilder.Entity<RequestStatus>().HasData(
                new RequestStatus { RequestStatusId = 1, RequestStatusName = "Pending", Description = "Pending approval" },
                new RequestStatus { RequestStatusId = 2, RequestStatusName = "Approved", Description = "Approved" },
                new RequestStatus { RequestStatusId = 3, RequestStatusName = "Rejected", Description = "Rejected" }
            );

            // Seed Borrow Statuses
            modelBuilder.Entity<BorrowStatus>().HasData(
                new BorrowStatus { BorrowStatusId = 1, BorrowStatusName = "Active", Description = "Active borrow" },
                new BorrowStatus { BorrowStatusId = 2, BorrowStatusName = "Returned", Description = "Item returned" },
                new BorrowStatus { BorrowStatusId = 3, BorrowStatusName = "Overdue", Description = "Overdue" },
                new BorrowStatus { BorrowStatusId = 4, BorrowStatusName = "Lost", Description = "Item lost" }
            );
        }
    }
}
