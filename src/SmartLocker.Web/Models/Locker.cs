using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocker.Web.Models
{
    public class Locker
    {
        [Key]
        public int LockerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LockerName { get; set; }

        [MaxLength(100)]
        public string Location { get; set; }

        [ForeignKey("LockerStatus")]
        public int LockerStatusId { get; set; }

        [MaxLength(50)]
        public string GpioPin { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual LockerStatus LockerStatus { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
        public virtual ICollection<LockerAccessToken> AccessTokens { get; set; } = new List<LockerAccessToken>();
    }
}
