using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLocker.Web.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        [ForeignKey("RequestStatus")]
        public int RequestStatusId { get; set; }

        [MaxLength(500)]
        public string Justification { get; set; }

        [MaxLength(500)]
        public string RejectionReason { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovedAt { get; set; }

        public DateTime? RejectedAt { get; set; }

        public DateTime? RequestedStartDate { get; set; }

        public DateTime? RequestedEndDate { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Item Item { get; set; }
        public virtual RequestStatus RequestStatus { get; set; }
        public virtual Borrow Borrow { get; set; }
    }
}
