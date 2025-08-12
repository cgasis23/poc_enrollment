using FluentValidation;
using EnrollmentApi.DTOs;

namespace EnrollmentApi.Validators
{
    public class MfaVerifyDtoValidator : AbstractValidator<MfaVerifyDto>
    {
        public MfaVerifyDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Customer ID must be greater than 0");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("MFA code is required")
                .Length(6).WithMessage("MFA code must be exactly 6 digits")
                .Matches(@"^\d{6}$").WithMessage("MFA code must contain only digits");
        }
    }
}
