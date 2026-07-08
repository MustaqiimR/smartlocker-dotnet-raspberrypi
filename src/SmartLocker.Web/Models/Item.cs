using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocker.Web.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("ItemStatus")]
        public int ItemStatusId { get; set; }

        [ForeignKey("Locker")]
        public int? LockerId { get; set; }

        [MaxLength(100)]
        public string SerialNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual ItemStatus ItemStatus { get; set; }
        public virtual Locker Locker { get; set; }
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    }
}
