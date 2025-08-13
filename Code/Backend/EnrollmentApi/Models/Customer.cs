namespace EnrollmentApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? AccountNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; } = "US";
        public DateTime DateOfBirth { get; set; }
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
