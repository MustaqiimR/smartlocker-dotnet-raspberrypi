using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocker.Web.Models
{
    public class BorrowLog
    {
        [Key]
        public int BorrowLogId { get; set; }

        [ForeignKey("Borrow")]
        public int? BorrowId { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } // "Requested", "Approved", "Rejected", "Borrowed", "Returned", "Extended", "Overdue", "Lost"

        [MaxLength(500)]
        public string ActionDetails { get; set; }

        [MaxLength(100)]
        public string PreviousStatus { get; set; }

        [MaxLength(100)]
        public string NewStatus { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Borrow Borrow { get; set; }
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}
