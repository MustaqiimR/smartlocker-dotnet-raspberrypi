using System.ComponentModel.DataAnnotations;

namespace SmartLocker.Web.Models
{
    public class BorrowStatus
    {
        [Key]
        public int BorrowStatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BorrowStatusName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    }
}
