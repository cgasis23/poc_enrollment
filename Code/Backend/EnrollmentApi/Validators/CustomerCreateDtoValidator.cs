using FluentValidation;
using EnrollmentApi.DTOs;

namespace EnrollmentApi.Validators
{
    public class CustomerCreateDtoValidator : AbstractValidator<CustomerCreateDto>
    {
        public CustomerCreateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters")
                .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("First name can only contain letters, spaces, hyphens, and apostrophes");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters")
                .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Last name can only contain letters, spaces, hyphens, and apostrophes");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please provide a valid email address")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^[\+]?[1-9][\d]{0,15}$").WithMessage("Please provide a valid phone number");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateTime.Today.AddYears(-13)).WithMessage("Customer must be at least 13 years old")
                .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Please provide a valid date of birth");

            RuleFor(x => x.Ssn)
                .Matches(@"^\d{3}-?\d{2}-?\d{4}$").When(x => !string.IsNullOrEmpty(x.Ssn))
                .WithMessage("SSN must be in format XXX-XX-XXXX or XXXXXXXXX");

            RuleFor(x => x.Address)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Address))
                .WithMessage("Address cannot exceed 200 characters");

            RuleFor(x => x.City)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.City))
                .WithMessage("City cannot exceed 50 characters");

            RuleFor(x => x.State)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.State))
                .WithMessage("State cannot exceed 50 characters");

            RuleFor(x => x.ZipCode)
                .Matches(@"^\d{5}(-\d{4})?$").When(x => !string.IsNullOrEmpty(x.ZipCode))
                .WithMessage("Zip code must be in format XXXXX or XXXXX-XXXX");

            RuleFor(x => x.Country)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Country))
                .WithMessage("Country cannot exceed 50 characters");
        }
    }
}
