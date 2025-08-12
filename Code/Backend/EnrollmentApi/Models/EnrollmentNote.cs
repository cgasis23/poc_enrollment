using System.ComponentModel.DataAnnotations;

namespace EnrollmentApi.Models
{
    public class EnrollmentNote
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Author { get; set; }
        
        public NoteType Type { get; set; } = NoteType.General;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsInternal { get; set; } = false;
        
        // Navigation property
        public virtual Customer Customer { get; set; } = null!;
    }
    
    public enum NoteType
    {
        General,
        Verification,
        Document,
        MFA,
        System,
        Customer
    }
}
