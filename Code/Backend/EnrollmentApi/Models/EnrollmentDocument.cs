using System.ComponentModel.DataAnnotations;

namespace EnrollmentApi.Models
{
    public class EnrollmentDocument
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string DocumentType { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public string FilePath { get; set; } = string.Empty;
        
        public long FileSize { get; set; }
        
        [StringLength(100)]
        public string ContentType { get; set; } = string.Empty;
        
        public DocumentStatus Status { get; set; } = DocumentStatus.Pending;
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ProcessedAt { get; set; }
        
        [StringLength(500)]
        public string? RejectionReason { get; set; }
        
        // Navigation property
        public virtual Customer Customer { get; set; } = null!;
    }
    
    public enum DocumentStatus
    {
        Pending,
        Approved,
        Rejected,
        UnderReview
    }
}
