using System.ComponentModel.DataAnnotations;

namespace EnrollmentApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? Address { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        [StringLength(50)]
        public string? State { get; set; }
        
        [StringLength(20)]
        public string? ZipCode { get; set; }
        
        [StringLength(50)]
        public string? Country { get; set; } = "US";
        
        public DateTime DateOfBirth { get; set; }
        
        [StringLength(20)]
        public string? Ssn { get; set; }
        
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsMfaEnabled { get; set; } = false;
        
        public string? MfaSecret { get; set; }
        
        public DateTime? MfaEnabledAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<EnrollmentDocument> Documents { get; set; } = new List<EnrollmentDocument>();
        public virtual ICollection<EnrollmentNote> Notes { get; set; } = new List<EnrollmentNote>();
    }
    
    public enum EnrollmentStatus
    {
        Pending,
        InProgress,
        Completed,
        Rejected,
        Cancelled
    }
}
