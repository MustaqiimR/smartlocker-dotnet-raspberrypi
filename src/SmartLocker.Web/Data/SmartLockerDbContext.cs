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
        }
    }
}
