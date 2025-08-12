using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using EnrollmentApi.Data;
using EnrollmentApi.DTOs;
using EnrollmentApi.Models;

namespace EnrollmentApi.Services
{
    public class MfaService : IMfaService
    {
        private readonly EnrollmentDbContext _context;
        private const string Issuer = "EnrollmentAPI";
        private const string Algorithm = "SHA1";
        private const int Digits = 6;
        private const int Period = 30;

        public MfaService(EnrollmentDbContext context)
        {
            _context = context;
        }

        public async Task<MfaSetupDto> SetupMfaAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                throw new ArgumentException($"Customer with ID {customerId} not found.");

            if (customer.IsMfaEnabled)
                throw new InvalidOperationException("MFA is already enabled for this customer.");

            var secret = await GenerateSecretAsync();
            var qrCodeUrl = await GenerateQrCodeUrlAsync(secret, customer.Email);
            var backupCodes = await GenerateBackupCodesAsync();

            // Store the secret temporarily (it will be saved when MFA is enabled)
            customer.MfaSecret = secret;

            await _context.SaveChangesAsync();

            return new MfaSetupDto
            {
                CustomerId = customerId,
                Secret = secret,
                QrCodeUrl = qrCodeUrl,
                BackupCodes = string.Join(",", backupCodes)
            };
        }

        public async Task<bool> VerifyMfaCodeAsync(int customerId, string code)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null || !customer.IsMfaEnabled || string.IsNullOrEmpty(customer.MfaSecret))
                return false;

            var currentCode = GenerateTotpCode(customer.MfaSecret);
            return code == currentCode;
        }

        public async Task<bool> EnableMfaAsync(int customerId, string code)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null || string.IsNullOrEmpty(customer.MfaSecret))
                return false;

            var currentCode = GenerateTotpCode(customer.MfaSecret);
            if (code != currentCode)
                return false;

            customer.IsMfaEnabled = true;
            customer.MfaEnabledAt = DateTime.UtcNow;
            customer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisableMfaAsync(int customerId, string code)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null || !customer.IsMfaEnabled || string.IsNullOrEmpty(customer.MfaSecret))
                return false;

            var currentCode = GenerateTotpCode(customer.MfaSecret);
            if (code != currentCode)
                return false;

            customer.IsMfaEnabled = false;
            customer.MfaSecret = null;
            customer.MfaEnabledAt = null;
            customer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MfaStatusDto> GetMfaStatusAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                throw new ArgumentException($"Customer with ID {customerId} not found.");

            return new MfaStatusDto
            {
                CustomerId = customerId,
                IsEnabled = customer.IsMfaEnabled,
                EnabledAt = customer.MfaEnabledAt,
                Secret = customer.MfaSecret,
                QrCodeUrl = customer.IsMfaEnabled && !string.IsNullOrEmpty(customer.MfaSecret) 
                    ? await GenerateQrCodeUrlAsync(customer.MfaSecret, customer.Email) 
                    : null
            };
        }

        public Task<string> GenerateQrCodeUrlAsync(string secret, string email)
        {
            var label = Uri.EscapeDataString(email);
            var issuer = Uri.EscapeDataString(Issuer);
            var secretParam = Uri.EscapeDataString(secret);
            
            return Task.FromResult($"otpauth://totp/{issuer}:{label}?secret={secretParam}&issuer={issuer}&algorithm={Algorithm}&digits={Digits}&period={Period}");
        }

        public Task<string> GenerateSecretAsync()
        {
            var randomBytes = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            
            var secret = Convert.ToBase64String(randomBytes)
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "")
                .Substring(0, 32);
                
            return Task.FromResult(secret);
        }

        public Task<string[]> GenerateBackupCodesAsync()
        {
            var codes = new string[10];
            using (var rng = RandomNumberGenerator.Create())
            {
                for (int i = 0; i < 10; i++)
                {
                    var bytes = new byte[4];
                    rng.GetBytes(bytes);
                    var number = BitConverter.ToUInt32(bytes, 0);
                    codes[i] = (number % 1000000).ToString("D6");
                }
            }
            return Task.FromResult(codes);
        }

        private string GenerateTotpCode(string secret)
        {
            var counter = (ulong)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() / Period);
            var counterBytes = BitConverter.GetBytes(counter);
            
            if (BitConverter.IsLittleEndian)
                Array.Reverse(counterBytes);

            var key = Convert.FromBase64String(secret + "=="); // Add padding
            using var hmac = new HMACSHA1(key);
            var hash = hmac.ComputeHash(counterBytes);
            
            var offset = hash[^1] & 0xf;
            var binary = ((hash[offset] & 0x7f) << 24) |
                        ((hash[offset + 1] & 0xff) << 16) |
                        ((hash[offset + 2] & 0xff) << 8) |
                        (hash[offset + 3] & 0xff);
            
            var code = binary % (int)Math.Pow(10, Digits);
            return code.ToString($"D{Digits}");
        }
    }
}
