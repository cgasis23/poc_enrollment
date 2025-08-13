using FluentAssertions;
using FluentValidation.TestHelper;
using EnrollmentApi.Validators;
using EnrollmentApi.DTOs;
using EnrollmentApi.Models;

namespace EnrollmentApi.Tests.Unit
{
    public class ValidatorTests
    {
        private readonly CustomerCreateDtoValidator _customerCreateValidator;
        private readonly CustomerUpdateDtoValidator _customerUpdateValidator;
        private readonly MfaEnableDtoValidator _mfaEnableValidator;
        private readonly MfaVerifyDtoValidator _mfaVerifyValidator;

        public ValidatorTests()
        {
            _customerCreateValidator = new CustomerCreateDtoValidator();
            _customerUpdateValidator = new CustomerUpdateDtoValidator();
            _mfaEnableValidator = new MfaEnableDtoValidator();
            _mfaVerifyValidator = new MfaVerifyDtoValidator();
        }

        #region CustomerCreateDtoValidator Tests

        [Fact]
        public void CustomerCreateDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Ssn = "123-45-6789",
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                Country = "US"
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "First name is required")]
        [InlineData("John123", "First name can only contain letters, spaces, hyphens, and apostrophes")]
        [InlineData("Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "First name cannot exceed 50 characters")]
        public void CustomerCreateDto_WithInvalidFirstName_ShouldFailValidation(string firstName, string expectedError)
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = firstName,
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25)
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("", "Last name is required")]
        [InlineData("Doe123", "Last name can only contain letters, spaces, hyphens, and apostrophes")]
        [InlineData("Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "Last name cannot exceed 50 characters")]
        public void CustomerCreateDto_WithInvalidLastName_ShouldFailValidation(string lastName, string expectedError)
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = "John",
                LastName = lastName,
                Email = "john.doe@example.com",
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25)
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("", "Email is required")]
        [InlineData("invalid-email", "Please provide a valid email address")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@example.com", "Email cannot exceed 100 characters")]
        public void CustomerCreateDto_WithInvalidEmail_ShouldFailValidation(string email, string expectedError)
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25)
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("", "Phone number is required")]
        [InlineData("invalid", "Please provide a valid phone number")]
        [InlineData("0123456789", "Please provide a valid phone number")]
        public void CustomerCreateDto_WithInvalidPhoneNumber_ShouldFailValidation(string phoneNumber, string expectedError)
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = phoneNumber,
                DateOfBirth = DateTime.Today.AddYears(-25)
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("", "Date of birth is required")]
        [InlineData("2020-01-01", "Customer must be at least 13 years old")]
        [InlineData("1800-01-01", "Please provide a valid date of birth")]
        public void CustomerCreateDto_WithInvalidDateOfBirth_ShouldFailValidation(string dateOfBirth, string expectedError)
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "5551234567",
                DateOfBirth = string.IsNullOrEmpty(dateOfBirth) ? default : DateTime.Parse(dateOfBirth)
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("12-34-5678", "SSN must be in format XXX-XX-XXXX or XXXXXXXXX")]
        [InlineData("123-4-5678", "SSN must be in format XXX-XX-XXXX or XXXXXXXXX")]
        public void CustomerCreateDto_WithInvalidSSN_ShouldFailValidation(string ssn, string expectedError)
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Ssn = ssn
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Ssn)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("123-45-6789")]
        [InlineData("123456789")]
        public void CustomerCreateDto_WithValidSSN_ShouldPassValidation(string ssn)
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Ssn = ssn
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Ssn);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("12345-6789")]
        public void CustomerCreateDto_WithValidZipCode_ShouldPassValidation(string zipCode)
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25),
                ZipCode = zipCode
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ZipCode);
        }

        [Theory]
        [InlineData("1234", "Zip code must be in format XXXXX or XXXXX-XXXX")]
        [InlineData("123456", "Zip code must be in format XXXXX or XXXXX-XXXX")]
        [InlineData("1234-567", "Zip code must be in format XXXXX or XXXXX-XXXX")]
        public void CustomerCreateDto_WithInvalidZipCode_ShouldFailValidation(string zipCode, string expectedError)
        {
            // Arrange
            var dto = new CustomerCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25),
                ZipCode = zipCode
            };

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ZipCode)
                  .WithErrorMessage(expectedError);
        }

        #endregion

        #region CustomerUpdateDtoValidator Tests

        [Fact]
        public void CustomerUpdateDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new CustomerUpdateDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Ssn = "123-45-6789",
                Status = EnrollmentStatus.Pending
            };

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void CustomerUpdateDto_WithEmptyData_ShouldFailValidation()
        {
            // Arrange
            var dto = new CustomerUpdateDto();

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            // Empty DTO should fail validation since FirstName and LastName are required
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage("First name is required");
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage("Last name is required");
        }

        [Theory]
        [InlineData("", "First name is required")]
        [InlineData("John123", "First name can only contain letters, spaces, hyphens, and apostrophes")]
        public void CustomerUpdateDto_WithInvalidFirstName_ShouldFailValidation(string firstName, string expectedError)
        {
            // Arrange
            var dto = new CustomerUpdateDto
            {
                FirstName = firstName,
                LastName = "Doe" // Provide valid LastName to avoid validation errors
            };

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("", "Last name is required")]
        [InlineData("Doe123", "Last name can only contain letters, spaces, hyphens, and apostrophes")]
        public void CustomerUpdateDto_WithInvalidLastName_ShouldFailValidation(string lastName, string expectedError)
        {
            // Arrange
            var dto = new CustomerUpdateDto
            {
                FirstName = "John", // Provide valid FirstName to avoid validation errors
                LastName = lastName
            };

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("invalid", "Please provide a valid phone number")]
        [InlineData("0123456789", "Please provide a valid phone number")]
        public void CustomerUpdateDto_WithInvalidPhoneNumber_ShouldFailValidation(string phoneNumber, string expectedError)
        {
            // Arrange
            var dto = new CustomerUpdateDto
            {
                FirstName = "John", // Provide valid FirstName to avoid validation errors
                LastName = "Doe",   // Provide valid LastName to avoid validation errors
                PhoneNumber = phoneNumber
            };

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("2020-01-01", "Customer must be at least 13 years old")]
        [InlineData("1800-01-01", "Please provide a valid date of birth")]
        public void CustomerUpdateDto_WithInvalidDateOfBirth_ShouldFailValidation(string dateOfBirth, string expectedError)
        {
            // Arrange
            var dto = new CustomerUpdateDto
            {
                DateOfBirth = DateTime.Parse(dateOfBirth)
            };

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("12-34-5678", "SSN must be in format XXX-XX-XXXX or XXXXXXXXX")]
        [InlineData("123-4-5678", "SSN must be in format XXX-XX-XXXX or XXXXXXXXX")]
        public void CustomerUpdateDto_WithInvalidSSN_ShouldFailValidation(string ssn, string expectedError)
        {
            // Arrange
            var dto = new CustomerUpdateDto
            {
                FirstName = "John", // Provide valid FirstName to avoid validation errors
                LastName = "Doe",   // Provide valid LastName to avoid validation errors
                Ssn = ssn
            };

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Ssn)
                  .WithErrorMessage(expectedError);
        }

        [Fact]
        public void CustomerUpdateDto_WithInvalidStatus_ShouldFailValidation()
        {
            // Arrange
            var dto = new CustomerUpdateDto
            {
                Status = (EnrollmentStatus)999 // Invalid enum value
            };

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Status)
                  .WithErrorMessage("Please provide a valid enrollment status");
        }

        #endregion

        #region MfaEnableDtoValidator Tests

        [Fact]
        public void MfaEnableDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new MfaEnableDto
            {
                CustomerId = 1,
                Code = "123456"
            };

            // Act
            var result = _mfaEnableValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0, "Customer ID must be greater than 0")]
        [InlineData(-1, "Customer ID must be greater than 0")]
        public void MfaEnableDto_WithInvalidCustomerId_ShouldFailValidation(int customerId, string expectedError)
        {
            // Arrange
            var dto = new MfaEnableDto
            {
                CustomerId = customerId,
                Code = "123456"
            };

            // Act
            var result = _mfaEnableValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CustomerId)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("", "MFA code is required")]
        [InlineData("12345", "MFA code must be exactly 6 digits")]
        [InlineData("1234567", "MFA code must be exactly 6 digits")]
        [InlineData("12345a", "MFA code must contain only digits")]
        [InlineData("abcdef", "MFA code must contain only digits")]
        public void MfaEnableDto_WithInvalidCode_ShouldFailValidation(string code, string expectedError)
        {
            // Arrange
            var dto = new MfaEnableDto
            {
                CustomerId = 1,
                Code = code
            };

            // Act
            var result = _mfaEnableValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                  .WithErrorMessage(expectedError);
        }

        #endregion

        #region MfaVerifyDtoValidator Tests

        [Fact]
        public void MfaVerifyDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new MfaVerifyDto
            {
                CustomerId = 1,
                Code = "123456"
            };

            // Act
            var result = _mfaVerifyValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0, "Customer ID must be greater than 0")]
        [InlineData(-1, "Customer ID must be greater than 0")]
        public void MfaVerifyDto_WithInvalidCustomerId_ShouldFailValidation(int customerId, string expectedError)
        {
            // Arrange
            var dto = new MfaVerifyDto
            {
                CustomerId = customerId,
                Code = "123456"
            };

            // Act
            var result = _mfaVerifyValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CustomerId)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData("", "MFA code is required")]
        [InlineData("12345", "MFA code must be exactly 6 digits")]
        [InlineData("1234567", "MFA code must be exactly 6 digits")]
        [InlineData("12345a", "MFA code must contain only digits")]
        [InlineData("abcdef", "MFA code must contain only digits")]
        public void MfaVerifyDto_WithInvalidCode_ShouldFailValidation(string code, string expectedError)
        {
            // Arrange
            var dto = new MfaVerifyDto
            {
                CustomerId = 1,
                Code = code
            };

            // Act
            var result = _mfaVerifyValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                  .WithErrorMessage(expectedError);
        }

        #endregion
    }
}
