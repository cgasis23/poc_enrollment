using Microsoft.AspNetCore.Mvc;
using EnrollmentApi.Services;
using EnrollmentApi.DTOs;

namespace EnrollmentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MfaController : ControllerBase
    {
        private readonly IMfaService _mfaService;

        public MfaController(IMfaService mfaService)
        {
            _mfaService = mfaService;
        }

        /// <summary>
        /// Setup MFA for a customer
        /// </summary>
        [HttpPost("setup/{customerId}")]
        public async Task<ActionResult<MfaSetupDto>> SetupMfa(int customerId)
        {
            try
            {
                var setupResult = await _mfaService.SetupMfaAsync(customerId);
                return Ok(setupResult);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while setting up MFA.", details = ex.Message });
            }
        }

        /// <summary>
        /// Verify MFA code for a customer
        /// </summary>
        [HttpPost("verify")]
        public async Task<ActionResult<bool>> VerifyMfaCode(MfaVerifyDto verifyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var isValid = await _mfaService.VerifyMfaCodeAsync(verifyDto.CustomerId, verifyDto.Code);
                return Ok(new { isValid });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while verifying MFA code.", details = ex.Message });
            }
        }

        /// <summary>
        /// Enable MFA for a customer
        /// </summary>
        [HttpPost("enable")]
        public async Task<ActionResult<bool>> EnableMfa(MfaEnableDto enableDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _mfaService.EnableMfaAsync(enableDto.CustomerId, enableDto.Code);
                if (!success)
                    return BadRequest(new { error = "Invalid MFA code or customer not found." });

                return Ok(new { success = true, message = "MFA has been enabled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while enabling MFA.", details = ex.Message });
            }
        }

        /// <summary>
        /// Disable MFA for a customer
        /// </summary>
        [HttpPost("disable")]
        public async Task<ActionResult<bool>> DisableMfa(MfaDisableDto disableDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _mfaService.DisableMfaAsync(disableDto.CustomerId, disableDto.Code);
                if (!success)
                    return BadRequest(new { error = "Invalid MFA code or customer not found." });

                return Ok(new { success = true, message = "MFA has been disabled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while disabling MFA.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get MFA status for a customer
        /// </summary>
        [HttpGet("status/{customerId}")]
        public async Task<ActionResult<MfaStatusDto>> GetMfaStatus(int customerId)
        {
            try
            {
                var status = await _mfaService.GetMfaStatusAsync(customerId);
                return Ok(status);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving MFA status.", details = ex.Message });
            }
        }

        /// <summary>
        /// Generate QR code URL for MFA setup
        /// </summary>
        [HttpPost("qrcode")]
        public async Task<ActionResult<string>> GenerateQrCode([FromBody] QrCodeRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Secret) || string.IsNullOrEmpty(request.Email))
                    return BadRequest(new { error = "Secret and email are required." });

                var qrCodeUrl = await _mfaService.GenerateQrCodeUrlAsync(request.Secret, request.Email);
                return Ok(new { qrCodeUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while generating QR code.", details = ex.Message });
            }
        }

        /// <summary>
        /// Generate backup codes for MFA
        /// </summary>
        [HttpPost("backup-codes")]
        public async Task<ActionResult<string[]>> GenerateBackupCodes()
        {
            try
            {
                var backupCodes = await _mfaService.GenerateBackupCodesAsync();
                return Ok(new { backupCodes });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while generating backup codes.", details = ex.Message });
            }
        }
    }

    public class QrCodeRequest
    {
        public string Secret { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
