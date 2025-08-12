namespace EnrollmentApi.Models
{
    public class EnrollmentDocument
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public DocumentStatus Status { get; set; } = DocumentStatus.Pending;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
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
