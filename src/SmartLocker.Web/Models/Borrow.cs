using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocker.Web.Models
{
    public class Borrow
    {
        [Key]
        public int BorrowId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        [ForeignKey("Locker")]
        public int LockerId { get; set; }

        [ForeignKey("BorrowStatus")]
        public int BorrowStatusId { get; set; }

        public int? RequestId { get; set; }

        public DateTime BorrowStartDate { get; set; } = DateTime.UtcNow;

        public DateTime BorrowEndDate { get; set; }

        public DateTime? BorrowReturnDate { get; set; }

        public bool IsOverdue { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Item Item { get; set; }
        public virtual Locker Locker { get; set; }
        public virtual BorrowStatus BorrowStatus { get; set; }
        public virtual Request Request { get; set; }
        public virtual ICollection<LockerAccessToken> AccessTokens { get; set; } = new List<LockerAccessToken>();
    }
}
