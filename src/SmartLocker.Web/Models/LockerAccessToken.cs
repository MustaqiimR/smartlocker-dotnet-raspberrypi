using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocker.Web.Models
{
    public class LockerAccessToken
    {
        [Key]
        public int TokenId { get; set; }

        [ForeignKey("Borrow")]
        public int BorrowId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        [ForeignKey("Locker")]
        public int LockerId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Token { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime? UsedAt { get; set; }

        public bool IsValid { get; set; } = true;

        // Purpose: BorrowUnlock, ReturnUnlock, AdminUnlock
        [Required]
        [MaxLength(50)]
        public string Purpose { get; set; } = "BorrowUnlock";

        [ForeignKey("CreatedByUser")]
        public int? CreatedByUserId { get; set; }

        public int FailedAttemptCount { get; set; } = 0;

        // Navigation properties
        public virtual Borrow Borrow { get; set; }
        public virtual User User { get; set; }
        public virtual Item Item { get; set; }
        public virtual Locker Locker { get; set; }
        public virtual User CreatedByUser { get; set; }
    }
}
