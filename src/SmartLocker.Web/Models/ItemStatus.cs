using System.ComponentModel.DataAnnotations;

namespace SmartLocker.Web.Models
{
    public class ItemStatus
    {
        [Key]
        public int ItemStatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ItemStatusName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
