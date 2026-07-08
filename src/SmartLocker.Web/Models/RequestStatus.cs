using System.ComponentModel.DataAnnotations;

namespace SmartLocker.Web.Models
{
    public class RequestStatus
    {
        [Key]
        public int RequestStatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RequestStatusName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
