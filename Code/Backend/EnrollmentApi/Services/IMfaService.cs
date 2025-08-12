using EnrollmentApi.DTOs;

namespace EnrollmentApi.Services
{
    public interface IMfaService
    {
        Task<MfaSetupDto> SetupMfaAsync(int customerId);
        Task<bool> VerifyMfaCodeAsync(int customerId, string code);
        Task<bool> EnableMfaAsync(int customerId, string code);
        Task<bool> DisableMfaAsync(int customerId, string code);
        Task<MfaStatusDto> GetMfaStatusAsync(int customerId);
        Task<string> GenerateQrCodeUrlAsync(string secret, string email);
        Task<string> GenerateSecretAsync();
        Task<string[]> GenerateBackupCodesAsync();
    }
}
