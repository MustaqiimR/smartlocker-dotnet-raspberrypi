using System.ComponentModel.DataAnnotations;

namespace SmartLocker.Web.Models
{
    public class LockerStatus
    {
        [Key]
        public int LockerStatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LockerStatusName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<Locker> Lockers { get; set; } = new List<Locker>();
    }
}
