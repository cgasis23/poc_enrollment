using System.ComponentModel.DataAnnotations;

namespace EnrollmentApi.DTOs
{
    public class MfaSetupDto
    {
        public int CustomerId { get; set; }
        public string Secret { get; set; } = string.Empty;
        public string QrCodeUrl { get; set; } = string.Empty;
        public string BackupCodes { get; set; } = string.Empty;
    }

    public class MfaVerifyDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = string.Empty;
    }

    public class MfaEnableDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = string.Empty;
    }

    public class MfaStatusDto
    {
        public int CustomerId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? EnabledAt { get; set; }
        public string? Secret { get; set; }
        public string? QrCodeUrl { get; set; }
    }

    public class MfaDisableDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = string.Empty;
    }
}
