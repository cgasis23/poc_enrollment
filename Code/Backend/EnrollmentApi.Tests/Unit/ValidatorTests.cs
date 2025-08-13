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
            var dto = TestDataBuilder.CreateCustomerCreateDto(CustomerCreateDtoRecipe.ValidCustomer);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(CustomerCreateDtoRecipe.EmptyFirstName, "First name is required")]
        [InlineData(CustomerCreateDtoRecipe.InvalidFirstName, "First name can only contain letters, spaces, hyphens, and apostrophes")]
        [InlineData(CustomerCreateDtoRecipe.LongFirstName, "First name cannot exceed 50 characters")]
        public void CustomerCreateDto_WithInvalidFirstName_ShouldFailValidation(CustomerCreateDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerCreateDto(recipe);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData(CustomerCreateDtoRecipe.EmptyLastName, "Last name is required")]
        [InlineData(CustomerCreateDtoRecipe.InvalidLastName, "Last name can only contain letters, spaces, hyphens, and apostrophes")]
        [InlineData(CustomerCreateDtoRecipe.LongLastName, "Last name cannot exceed 50 characters")]
        public void CustomerCreateDto_WithInvalidLastName_ShouldFailValidation(CustomerCreateDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerCreateDto(recipe);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData(CustomerCreateDtoRecipe.EmptyEmail, "Email is required")]
        [InlineData(CustomerCreateDtoRecipe.InvalidEmail, "Please provide a valid email address")]
        [InlineData(CustomerCreateDtoRecipe.LongEmail, "Email cannot exceed 100 characters")]
        public void CustomerCreateDto_WithInvalidEmail_ShouldFailValidation(CustomerCreateDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerCreateDto(recipe);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData(CustomerCreateDtoRecipe.EmptyPhoneNumber, "Phone number is required")]
        [InlineData(CustomerCreateDtoRecipe.InvalidPhoneNumber, "Please provide a valid phone number")]
        [InlineData(CustomerCreateDtoRecipe.InvalidPhoneNumberFormat, "Please provide a valid phone number")]
        public void CustomerCreateDto_WithInvalidPhoneNumber_ShouldFailValidation(CustomerCreateDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerCreateDto(recipe);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData(CustomerCreateDtoRecipe.EmptyDateOfBirth, "Date of birth is required")]
        [InlineData(CustomerCreateDtoRecipe.UnderageCustomer, "Customer must be at least 13 years old")]
        [InlineData(CustomerCreateDtoRecipe.InvalidDateOfBirth, "Please provide a valid date of birth")]
        public void CustomerCreateDto_WithInvalidDateOfBirth_ShouldFailValidation(CustomerCreateDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerCreateDto(recipe);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData(CustomerCreateDtoRecipe.InvalidSSN, "SSN must be in format XXX-XX-XXXX or XXXXXXXXX")]
        public void CustomerCreateDto_WithInvalidSSN_ShouldFailValidation(CustomerCreateDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerCreateDto(recipe);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Ssn)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData(CustomerCreateDtoRecipe.ValidSSN)]
        public void CustomerCreateDto_WithValidSSN_ShouldPassValidation(CustomerCreateDtoRecipe recipe)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerCreateDto(recipe);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Ssn);
        }

        [Theory]
        [InlineData(CustomerCreateDtoRecipe.InvalidZipCode, "Zip code must be in format XXXXX or XXXXX-XXXX")]
        public void CustomerCreateDto_WithInvalidZipCode_ShouldFailValidation(CustomerCreateDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerCreateDto(recipe);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ZipCode)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData(CustomerCreateDtoRecipe.ValidZipCode)]
        public void CustomerCreateDto_WithValidZipCode_ShouldPassValidation(CustomerCreateDtoRecipe recipe)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerCreateDto(recipe);

            // Act
            var result = _customerCreateValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ZipCode);
        }

        #endregion

        #region CustomerUpdateDtoValidator Tests

        [Fact]
        public void CustomerUpdateDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerUpdateDto(CustomerUpdateDtoRecipe.ValidCustomer);

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void CustomerUpdateDto_WithEmptyData_ShouldFailValidation()
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerUpdateDto(CustomerUpdateDtoRecipe.EmptyData);

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage("First name is required");
        }

        [Theory]
        [InlineData(CustomerUpdateDtoRecipe.InvalidFirstName)]
        [InlineData(CustomerUpdateDtoRecipe.InvalidLastName)]
        [InlineData(CustomerUpdateDtoRecipe.InvalidPhoneNumber)]
        [InlineData(CustomerUpdateDtoRecipe.InvalidDateOfBirth)]
        [InlineData(CustomerUpdateDtoRecipe.InvalidSSN)]
        public void CustomerUpdateDto_WithInvalidData_ShouldFailValidation(CustomerUpdateDtoRecipe recipe)
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerUpdateDto(recipe);

            // Act
            var result = _customerUpdateValidator.TestValidate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CustomerUpdateDto_WithInvalidStatus_ShouldFailValidation()
        {
            // Arrange
            var dto = TestDataBuilder.CreateCustomerUpdateDto(CustomerUpdateDtoRecipe.InvalidStatus);

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
            var dto = TestDataBuilder.CreateMfaEnableDto(MfaEnableDtoRecipe.ValidMfa);

            // Act
            var result = _mfaEnableValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(MfaEnableDtoRecipe.InvalidCustomerId, "Customer ID must be greater than 0")]
        public void MfaEnableDto_WithInvalidCustomerId_ShouldFailValidation(MfaEnableDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateMfaEnableDto(recipe);

            // Act
            var result = _mfaEnableValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CustomerId)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData(MfaEnableDtoRecipe.EmptyMfaCode, "MFA code is required")]
        [InlineData(MfaEnableDtoRecipe.LongMfaCode, "MFA code must be exactly 6 digits")]
        [InlineData(MfaEnableDtoRecipe.NonNumericMfaCode, "MFA code must contain only digits")]
        [InlineData(MfaEnableDtoRecipe.InvalidMfaCode, "MFA code must contain only digits")]
        public void MfaEnableDto_WithInvalidMfaCode_ShouldFailValidation(MfaEnableDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateMfaEnableDto(recipe);

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
            var dto = TestDataBuilder.CreateMfaVerifyDto(MfaVerifyDtoRecipe.ValidMfa);

            // Act
            var result = _mfaVerifyValidator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(MfaVerifyDtoRecipe.InvalidCustomerId, "Customer ID must be greater than 0")]
        public void MfaVerifyDto_WithInvalidCustomerId_ShouldFailValidation(MfaVerifyDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateMfaVerifyDto(recipe);

            // Act
            var result = _mfaVerifyValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CustomerId)
                  .WithErrorMessage(expectedError);
        }

        [Theory]
        [InlineData(MfaVerifyDtoRecipe.EmptyMfaCode, "MFA code is required")]
        [InlineData(MfaVerifyDtoRecipe.LongMfaCode, "MFA code must be exactly 6 digits")]
        [InlineData(MfaVerifyDtoRecipe.NonNumericMfaCode, "MFA code must contain only digits")]
        [InlineData(MfaVerifyDtoRecipe.InvalidMfaCode, "MFA code must contain only digits")]
        public void MfaVerifyDto_WithInvalidMfaCode_ShouldFailValidation(MfaVerifyDtoRecipe recipe, string expectedError)
        {
            // Arrange
            var dto = TestDataBuilder.CreateMfaVerifyDto(recipe);

            // Act
            var result = _mfaVerifyValidator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                  .WithErrorMessage(expectedError);
        }

        #endregion
    }
}
