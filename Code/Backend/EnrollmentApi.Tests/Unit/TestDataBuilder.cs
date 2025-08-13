using EnrollmentApi.DTOs;
using EnrollmentApi.Models;

namespace EnrollmentApi.Tests.Unit
{
    /// <summary>
    /// Recipe enums for different test scenarios
    /// </summary>
    public enum CustomerRecipe
    {
        ValidCustomer,
        CustomerWithSpacesInAccountNumber,
        InvalidAccountNumber,
        InvalidSSN,
        InvalidBirthdate,
        InvalidDateFormat,
        MultipleCustomers,
        StrictMockCustomer,
        CompletedCustomer,
        PendingCustomer,
        CancelledCustomer,
        CustomerWithLongName,
        CustomerWithSpecialCharacters
    }

    public enum TestInputRecipe
    {
        ValidInputs,
        AccountNumberWithSpaces,
        InvalidAccountNumber,
        InvalidSSN,
        InvalidBirthdate,
        InvalidDateFormat,
        NoMatchingCustomer,
        EmptyInputs,
        NullInputs,
        WhitespaceInputs
    }

    /// <summary>
    /// Recipe enums for validation test scenarios
    /// </summary>
    public enum CustomerCreateDtoRecipe
    {
        ValidCustomer,
        EmptyFirstName,
        InvalidFirstName,
        LongFirstName,
        EmptyLastName,
        InvalidLastName,
        LongLastName,
        EmptyEmail,
        InvalidEmail,
        LongEmail,
        EmptyPhoneNumber,
        InvalidPhoneNumber,
        InvalidPhoneNumberFormat,
        EmptyDateOfBirth,
        UnderageCustomer,
        InvalidDateOfBirth,
        InvalidSSN,
        ValidSSN,
        InvalidZipCode,
        ValidZipCode
    }

    public enum CustomerUpdateDtoRecipe
    {
        ValidCustomer,
        EmptyData,
        InvalidFirstName,
        InvalidLastName,
        InvalidPhoneNumber,
        InvalidDateOfBirth,
        InvalidSSN,
        InvalidStatus
    }

    public enum MfaEnableDtoRecipe
    {
        ValidMfa,
        InvalidCustomerId,
        InvalidMfaCode,
        EmptyMfaCode,
        LongMfaCode,
        NonNumericMfaCode
    }

    public enum MfaVerifyDtoRecipe
    {
        ValidMfa,
        InvalidCustomerId,
        InvalidMfaCode,
        EmptyMfaCode,
        LongMfaCode,
        NonNumericMfaCode
    }

    /// <summary>
    /// Test data builder for creating consistent test data across all tests
    /// </summary>
    public static class TestDataBuilder
    {
        // Common test data constants
        private const string ValidAccountNumber = "1234567890123456";
        private const string ValidSSN = "123456789";
        private const string ValidBirthdate = "1990-01-01";
        private const string AccountNumberWithSpaces = "1234 5678 9012 3456";
        private const string InvalidAccountNumber = "9999999999999999";
        private const string InvalidSSN = "999999999";
        private const string InvalidBirthdate = "1985-05-15";
        private const string InvalidDateFormat = "invalid-date";

        /// <summary>
        /// Creates a CustomerDto based on the specified recipe
        /// </summary>
        public static CustomerDto CreateCustomerDto(CustomerRecipe recipe)
        {
            return recipe switch
            {
                CustomerRecipe.ValidCustomer => CreateValidCustomerDto(),
                CustomerRecipe.CustomerWithSpacesInAccountNumber => CreateCustomerWithSpacesInAccountNumberDto(),
                CustomerRecipe.InvalidAccountNumber => CreateInvalidAccountNumberCustomerDto(),
                CustomerRecipe.InvalidSSN => CreateInvalidSSNCustomerDto(),
                CustomerRecipe.InvalidBirthdate => CreateInvalidBirthdateCustomerDto(),
                CustomerRecipe.InvalidDateFormat => CreateInvalidDateFormatCustomerDto(),
                CustomerRecipe.MultipleCustomers => CreateMultipleCustomersDto(),
                CustomerRecipe.StrictMockCustomer => CreateStrictMockCustomerDto(),
                CustomerRecipe.CompletedCustomer => CreateCompletedCustomerDto(),
                CustomerRecipe.PendingCustomer => CreatePendingCustomerDto(),
                CustomerRecipe.CancelledCustomer => CreateCancelledCustomerDto(),
                CustomerRecipe.CustomerWithLongName => CreateCustomerWithLongNameDto(),
                CustomerRecipe.CustomerWithSpecialCharacters => CreateCustomerWithSpecialCharactersDto(),
                _ => throw new ArgumentException($"Unknown customer recipe: {recipe}")
            };
        }

        /// <summary>
        /// Creates a CustomerCreateDto based on the specified recipe
        /// </summary>
        public static CustomerCreateDto CreateCustomerCreateDto(CustomerCreateDtoRecipe recipe)
        {
            return recipe switch
            {
                CustomerCreateDtoRecipe.ValidCustomer => CreateValidCustomerCreateDto(),
                CustomerCreateDtoRecipe.EmptyFirstName => CreateEmptyFirstNameCustomerCreateDto(),
                CustomerCreateDtoRecipe.InvalidFirstName => CreateInvalidFirstNameCustomerCreateDto(),
                CustomerCreateDtoRecipe.LongFirstName => CreateLongFirstNameCustomerCreateDto(),
                CustomerCreateDtoRecipe.EmptyLastName => CreateEmptyLastNameCustomerCreateDto(),
                CustomerCreateDtoRecipe.InvalidLastName => CreateInvalidLastNameCustomerCreateDto(),
                CustomerCreateDtoRecipe.LongLastName => CreateLongLastNameCustomerCreateDto(),
                CustomerCreateDtoRecipe.EmptyEmail => CreateEmptyEmailCustomerCreateDto(),
                CustomerCreateDtoRecipe.InvalidEmail => CreateInvalidEmailCustomerCreateDto(),
                CustomerCreateDtoRecipe.LongEmail => CreateLongEmailCustomerCreateDto(),
                CustomerCreateDtoRecipe.EmptyPhoneNumber => CreateEmptyPhoneNumberCustomerCreateDto(),
                CustomerCreateDtoRecipe.InvalidPhoneNumber => CreateInvalidPhoneNumberCustomerCreateDto(),
                CustomerCreateDtoRecipe.InvalidPhoneNumberFormat => CreateInvalidPhoneNumberFormatCustomerCreateDto(),
                CustomerCreateDtoRecipe.EmptyDateOfBirth => CreateEmptyDateOfBirthCustomerCreateDto(),
                CustomerCreateDtoRecipe.UnderageCustomer => CreateUnderageCustomerCreateDto(),
                CustomerCreateDtoRecipe.InvalidDateOfBirth => CreateInvalidDateOfBirthCustomerCreateDto(),
                CustomerCreateDtoRecipe.InvalidSSN => CreateInvalidSSNCustomerCreateDto(),
                CustomerCreateDtoRecipe.ValidSSN => CreateValidSSNCustomerCreateDto(),
                CustomerCreateDtoRecipe.InvalidZipCode => CreateInvalidZipCodeCustomerCreateDto(),
                CustomerCreateDtoRecipe.ValidZipCode => CreateValidZipCodeCustomerCreateDto(),
                _ => throw new ArgumentException($"Unknown customer create dto recipe: {recipe}")
            };
        }

        /// <summary>
        /// Creates a CustomerUpdateDto based on the specified recipe
        /// </summary>
        public static CustomerUpdateDto CreateCustomerUpdateDto(CustomerUpdateDtoRecipe recipe)
        {
            return recipe switch
            {
                CustomerUpdateDtoRecipe.ValidCustomer => CreateValidCustomerUpdateDto(),
                CustomerUpdateDtoRecipe.EmptyData => CreateEmptyDataCustomerUpdateDto(),
                CustomerUpdateDtoRecipe.InvalidFirstName => CreateInvalidFirstNameCustomerUpdateDto(),
                CustomerUpdateDtoRecipe.InvalidLastName => CreateInvalidLastNameCustomerUpdateDto(),

                CustomerUpdateDtoRecipe.InvalidPhoneNumber => CreateInvalidPhoneNumberCustomerUpdateDto(),
                CustomerUpdateDtoRecipe.InvalidDateOfBirth => CreateInvalidDateOfBirthCustomerUpdateDto(),
                CustomerUpdateDtoRecipe.InvalidSSN => CreateInvalidSSNCustomerUpdateDto(),
                CustomerUpdateDtoRecipe.InvalidStatus => CreateInvalidStatusCustomerUpdateDto(),
                _ => throw new ArgumentException($"Unknown customer update dto recipe: {recipe}")
            };
        }

        /// <summary>
        /// Creates an MfaEnableDto based on the specified recipe
        /// </summary>
        public static MfaEnableDto CreateMfaEnableDto(MfaEnableDtoRecipe recipe)
        {
            return recipe switch
            {
                MfaEnableDtoRecipe.ValidMfa => CreateValidMfaEnableDto(),
                MfaEnableDtoRecipe.InvalidCustomerId => CreateInvalidCustomerIdMfaEnableDto(),
                MfaEnableDtoRecipe.InvalidMfaCode => CreateInvalidMfaCodeMfaEnableDto(),
                MfaEnableDtoRecipe.EmptyMfaCode => CreateEmptyMfaCodeMfaEnableDto(),
                MfaEnableDtoRecipe.LongMfaCode => CreateLongMfaCodeMfaEnableDto(),
                MfaEnableDtoRecipe.NonNumericMfaCode => CreateNonNumericMfaCodeMfaEnableDto(),
                _ => throw new ArgumentException($"Unknown MFA enable dto recipe: {recipe}")
            };
        }

        /// <summary>
        /// Creates an MfaVerifyDto based on the specified recipe
        /// </summary>
        public static MfaVerifyDto CreateMfaVerifyDto(MfaVerifyDtoRecipe recipe)
        {
            return recipe switch
            {
                MfaVerifyDtoRecipe.ValidMfa => CreateValidMfaVerifyDto(),
                MfaVerifyDtoRecipe.InvalidCustomerId => CreateInvalidCustomerIdMfaVerifyDto(),
                MfaVerifyDtoRecipe.InvalidMfaCode => CreateInvalidMfaCodeMfaVerifyDto(),
                MfaVerifyDtoRecipe.EmptyMfaCode => CreateEmptyMfaCodeMfaVerifyDto(),
                MfaVerifyDtoRecipe.LongMfaCode => CreateLongMfaCodeMfaVerifyDto(),
                MfaVerifyDtoRecipe.NonNumericMfaCode => CreateNonNumericMfaCodeMfaVerifyDto(),
                _ => throw new ArgumentException($"Unknown MFA verify dto recipe: {recipe}")
            };
        }

        /// <summary>
        /// Creates test input parameters based on the specified recipe
        /// </summary>
        public static (string accountNumber, string ssn, string birthdate) CreateTestInputs(TestInputRecipe recipe)
        {
            return recipe switch
            {
                TestInputRecipe.ValidInputs => (ValidAccountNumber, ValidSSN, ValidBirthdate),
                TestInputRecipe.AccountNumberWithSpaces => (AccountNumberWithSpaces, ValidSSN, ValidBirthdate),
                TestInputRecipe.InvalidAccountNumber => (InvalidAccountNumber, ValidSSN, ValidBirthdate),
                TestInputRecipe.InvalidSSN => (ValidAccountNumber, InvalidSSN, ValidBirthdate),
                TestInputRecipe.InvalidBirthdate => (ValidAccountNumber, ValidSSN, InvalidBirthdate),
                TestInputRecipe.InvalidDateFormat => (ValidAccountNumber, ValidSSN, InvalidDateFormat),
                TestInputRecipe.NoMatchingCustomer => (ValidAccountNumber, ValidSSN, ValidBirthdate),
                TestInputRecipe.EmptyInputs => ("", "", ""),
                TestInputRecipe.NullInputs => (null!, null!, null!),
                TestInputRecipe.WhitespaceInputs => ("   ", "   ", "   "),
                _ => throw new ArgumentException($"Unknown test input recipe: {recipe}")
            };
        }

        /// <summary>
        /// Creates a Customer entity based on the specified recipe
        /// </summary>
        public static Customer CreateCustomer(CustomerRecipe recipe)
        {
            return recipe switch
            {
                CustomerRecipe.ValidCustomer => CreateValidCustomer(),
                CustomerRecipe.CustomerWithSpacesInAccountNumber => CreateCustomerWithSpacesInAccountNumber(),
                CustomerRecipe.InvalidAccountNumber => CreateInvalidAccountNumberCustomer(),
                CustomerRecipe.InvalidSSN => CreateInvalidSSNCustomer(),
                CustomerRecipe.InvalidBirthdate => CreateInvalidBirthdateCustomer(),
                CustomerRecipe.InvalidDateFormat => CreateInvalidDateFormatCustomer(),
                CustomerRecipe.MultipleCustomers => CreateMultipleCustomers(),
                CustomerRecipe.StrictMockCustomer => CreateStrictMockCustomer(),
                CustomerRecipe.CompletedCustomer => CreateCompletedCustomer(),
                CustomerRecipe.PendingCustomer => CreatePendingCustomer(),
                CustomerRecipe.CancelledCustomer => CreateCancelledCustomer(),
                CustomerRecipe.CustomerWithLongName => CreateCustomerWithLongName(),
                CustomerRecipe.CustomerWithSpecialCharacters => CreateCustomerWithSpecialCharacters(),
                _ => throw new ArgumentException($"Unknown customer recipe: {recipe}")
            };
        }

        /// <summary>
        /// Creates a list of customers for multiple customer scenarios
        /// </summary>
        public static List<Customer> CreateMultipleCustomersList()
        {
            return new List<Customer>
            {
                CreateValidCustomer(),
                new Customer
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "555-987-6543",
                    AccountNumber = "9876543210987654",
                    Address = "456 Oak Ave",
                    City = "Somewhere",
                    State = "NY",
                    ZipCode = "10001",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    Ssn = "987654321",
                    Status = EnrollmentStatus.Completed,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }

        /// <summary>
        /// Fluent API for building CustomerDto objects
        /// </summary>
        public static CustomerDtoBuilder CustomerDto() => new();

        /// <summary>
        /// Fluent API for building Customer entities
        /// </summary>
        public static CustomerBuilder Customer() => new();

        #region Private Helper Methods

        private static CustomerDto CreateValidCustomerDto()
        {
            return new CustomerDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = ValidAccountNumber,
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = ValidSSN,
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static CustomerDto CreateCustomerWithSpacesInAccountNumberDto()
        {
            var dto = CreateValidCustomerDto();
            dto.AccountNumber = ValidAccountNumber; // Return without spaces
            return dto;
        }

        private static CustomerDto CreateInvalidAccountNumberCustomerDto()
        {
            var dto = CreateValidCustomerDto();
            dto.AccountNumber = InvalidAccountNumber;
            return dto;
        }

        private static CustomerDto CreateInvalidSSNCustomerDto()
        {
            var dto = CreateValidCustomerDto();
            dto.Ssn = InvalidSSN;
            return dto;
        }

        private static CustomerDto CreateInvalidBirthdateCustomerDto()
        {
            var dto = CreateValidCustomerDto();
            dto.DateOfBirth = new DateTime(1985, 5, 15);
            return dto;
        }

        private static CustomerDto CreateInvalidDateFormatCustomerDto()
        {
            var dto = CreateValidCustomerDto();
            // This would typically be null or default when date parsing fails
            dto.DateOfBirth = default;
            return dto;
        }

        private static CustomerDto CreateMultipleCustomersDto()
        {
            return CreateValidCustomerDto();
        }

        private static CustomerDto CreateStrictMockCustomerDto()
        {
            return CreateValidCustomerDto();
        }

        private static CustomerDto CreateCompletedCustomerDto()
        {
            var dto = CreateValidCustomerDto();
            dto.Status = EnrollmentStatus.Completed;
            return dto;
        }

        private static CustomerDto CreatePendingCustomerDto()
        {
            var dto = CreateValidCustomerDto();
            dto.Status = EnrollmentStatus.Pending;
            return dto;
        }

        private static CustomerDto CreateCancelledCustomerDto()
        {
            var dto = CreateValidCustomerDto();
            dto.Status = EnrollmentStatus.Cancelled;
            return dto;
        }

        private static CustomerDto CreateCustomerWithLongNameDto()
        {
            var dto = CreateValidCustomerDto();
            dto.FirstName = "VeryLongFirstNameThatExceedsNormalLength";
            dto.LastName = "VeryLongLastNameThatExceedsNormalLength";
            return dto;
        }

        private static CustomerDto CreateCustomerWithSpecialCharactersDto()
        {
            var dto = CreateValidCustomerDto();
            dto.FirstName = "José-María";
            dto.LastName = "O'Connor-Smith";
            dto.Email = "test+tag@example.com";
            return dto;
        }

        private static Customer CreateValidCustomer()
        {
            return new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = ValidAccountNumber,
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = ValidSSN,
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static Customer CreateCustomerWithSpacesInAccountNumber()
        {
            var customer = CreateValidCustomer();
            customer.AccountNumber = ValidAccountNumber; // Return without spaces
            return customer;
        }

        private static Customer CreateInvalidAccountNumberCustomer()
        {
            var customer = CreateValidCustomer();
            customer.AccountNumber = InvalidAccountNumber;
            return customer;
        }

        private static Customer CreateInvalidSSNCustomer()
        {
            var customer = CreateValidCustomer();
            customer.Ssn = InvalidSSN;
            return customer;
        }

        private static Customer CreateInvalidBirthdateCustomer()
        {
            var customer = CreateValidCustomer();
            customer.DateOfBirth = new DateTime(1985, 5, 15);
            return customer;
        }

        private static Customer CreateInvalidDateFormatCustomer()
        {
            var customer = CreateValidCustomer();
            customer.DateOfBirth = default;
            return customer;
        }

        private static Customer CreateMultipleCustomers()
        {
            return CreateValidCustomer();
        }

        private static Customer CreateStrictMockCustomer()
        {
            return CreateValidCustomer();
        }

        private static Customer CreateCompletedCustomer()
        {
            var customer = CreateValidCustomer();
            customer.Status = EnrollmentStatus.Completed;
            return customer;
        }

        private static Customer CreatePendingCustomer()
        {
            var customer = CreateValidCustomer();
            customer.Status = EnrollmentStatus.Pending;
            return customer;
        }

        private static Customer CreateCancelledCustomer()
        {
            var customer = CreateValidCustomer();
            customer.Status = EnrollmentStatus.Cancelled;
            return customer;
        }

        private static Customer CreateCustomerWithLongName()
        {
            var customer = CreateValidCustomer();
            customer.FirstName = "VeryLongFirstNameThatExceedsNormalLength";
            customer.LastName = "VeryLongLastNameThatExceedsNormalLength";
            return customer;
        }

        private static Customer CreateCustomerWithSpecialCharacters()
        {
            var customer = CreateValidCustomer();
            customer.FirstName = "José-María";
            customer.LastName = "O'Connor-Smith";
            customer.Email = "test+tag@example.com";
            return customer;
        }

        #endregion

        #region CustomerCreateDto Helper Methods

        private static CustomerCreateDto CreateValidCustomerCreateDto()
        {
            return new CustomerCreateDto
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
        }

        private static CustomerCreateDto CreateEmptyFirstNameCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.FirstName = "";
            return dto;
        }

        private static CustomerCreateDto CreateInvalidFirstNameCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.FirstName = "John123";
            return dto;
        }

        private static CustomerCreateDto CreateLongFirstNameCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.FirstName = "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            return dto;
        }

        private static CustomerCreateDto CreateEmptyLastNameCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.LastName = "";
            return dto;
        }

        private static CustomerCreateDto CreateInvalidLastNameCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.LastName = "Doe123";
            return dto;
        }

        private static CustomerCreateDto CreateLongLastNameCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.LastName = "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            return dto;
        }

        private static CustomerCreateDto CreateEmptyEmailCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.Email = "";
            return dto;
        }

        private static CustomerCreateDto CreateInvalidEmailCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.Email = "invalid-email";
            return dto;
        }

        private static CustomerCreateDto CreateLongEmailCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.Email = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@example.com";
            return dto;
        }

        private static CustomerCreateDto CreateEmptyPhoneNumberCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.PhoneNumber = "";
            return dto;
        }

        private static CustomerCreateDto CreateInvalidPhoneNumberCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.PhoneNumber = "invalid";
            return dto;
        }

        private static CustomerCreateDto CreateInvalidPhoneNumberFormatCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.PhoneNumber = "0123456789";
            return dto;
        }

        private static CustomerCreateDto CreateEmptyDateOfBirthCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.DateOfBirth = default;
            return dto;
        }

        private static CustomerCreateDto CreateUnderageCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.DateOfBirth = DateTime.Parse("2020-01-01");
            return dto;
        }

        private static CustomerCreateDto CreateInvalidDateOfBirthCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.DateOfBirth = DateTime.Parse("1800-01-01");
            return dto;
        }

        private static CustomerCreateDto CreateInvalidSSNCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.Ssn = "12-34-5678";
            return dto;
        }

        private static CustomerCreateDto CreateValidSSNCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.Ssn = "123456789";
            return dto;
        }

        private static CustomerCreateDto CreateInvalidZipCodeCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.ZipCode = "1234";
            return dto;
        }

        private static CustomerCreateDto CreateValidZipCodeCustomerCreateDto()
        {
            var dto = CreateValidCustomerCreateDto();
            dto.ZipCode = "12345-6789";
            return dto;
        }

        #endregion

        #region CustomerUpdateDto Helper Methods

        private static CustomerUpdateDto CreateValidCustomerUpdateDto()
        {
            return new CustomerUpdateDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "5551234567",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Ssn = "123-45-6789",
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                Status = EnrollmentStatus.Pending
            };
        }

        private static CustomerUpdateDto CreateEmptyDataCustomerUpdateDto()
        {
            return new CustomerUpdateDto();
        }

        private static CustomerUpdateDto CreateInvalidFirstNameCustomerUpdateDto()
        {
            var dto = CreateValidCustomerUpdateDto();
            dto.FirstName = "";
            return dto;
        }

        private static CustomerUpdateDto CreateInvalidLastNameCustomerUpdateDto()
        {
            var dto = CreateValidCustomerUpdateDto();
            dto.LastName = "Doe123";
            return dto;
        }

        private static CustomerUpdateDto CreateInvalidEmailCustomerUpdateDto()
        {
            var dto = CreateValidCustomerUpdateDto();
            dto.FirstName = "John123"; // Use invalid first name instead since Email doesn't exist
            return dto;
        }

        private static CustomerUpdateDto CreateInvalidPhoneNumberCustomerUpdateDto()
        {
            var dto = CreateValidCustomerUpdateDto();
            dto.PhoneNumber = "invalid";
            return dto;
        }

        private static CustomerUpdateDto CreateInvalidDateOfBirthCustomerUpdateDto()
        {
            var dto = CreateValidCustomerUpdateDto();
            dto.DateOfBirth = DateTime.Parse("2020-01-01");
            return dto;
        }

        private static CustomerUpdateDto CreateInvalidSSNCustomerUpdateDto()
        {
            var dto = CreateValidCustomerUpdateDto();
            dto.Ssn = "12-34-5678";
            return dto;
        }

        private static CustomerUpdateDto CreateInvalidStatusCustomerUpdateDto()
        {
            var dto = CreateValidCustomerUpdateDto();
            dto.Status = (EnrollmentStatus)999; // Invalid enum value
            return dto;
        }

        #endregion

        #region MfaEnableDto Helper Methods

        private static MfaEnableDto CreateValidMfaEnableDto()
        {
            return new MfaEnableDto
            {
                CustomerId = 1,
                Code = "123456"
            };
        }

        private static MfaEnableDto CreateInvalidCustomerIdMfaEnableDto()
        {
            var dto = CreateValidMfaEnableDto();
            dto.CustomerId = -1;
            return dto;
        }

        private static MfaEnableDto CreateInvalidMfaCodeMfaEnableDto()
        {
            var dto = CreateValidMfaEnableDto();
            dto.Code = "12345a";
            return dto;
        }

        private static MfaEnableDto CreateEmptyMfaCodeMfaEnableDto()
        {
            var dto = CreateValidMfaEnableDto();
            dto.Code = "";
            return dto;
        }

        private static MfaEnableDto CreateLongMfaCodeMfaEnableDto()
        {
            var dto = CreateValidMfaEnableDto();
            dto.Code = "1234567";
            return dto;
        }

        private static MfaEnableDto CreateNonNumericMfaCodeMfaEnableDto()
        {
            var dto = CreateValidMfaEnableDto();
            dto.Code = "abcdef";
            return dto;
        }

        #endregion

        #region MfaVerifyDto Helper Methods

        private static MfaVerifyDto CreateValidMfaVerifyDto()
        {
            return new MfaVerifyDto
            {
                CustomerId = 1,
                Code = "123456"
            };
        }

        private static MfaVerifyDto CreateInvalidCustomerIdMfaVerifyDto()
        {
            var dto = CreateValidMfaVerifyDto();
            dto.CustomerId = -1;
            return dto;
        }

        private static MfaVerifyDto CreateInvalidMfaCodeMfaVerifyDto()
        {
            var dto = CreateValidMfaVerifyDto();
            dto.Code = "12345a";
            return dto;
        }

        private static MfaVerifyDto CreateEmptyMfaCodeMfaVerifyDto()
        {
            var dto = CreateValidMfaVerifyDto();
            dto.Code = "";
            return dto;
        }

        private static MfaVerifyDto CreateLongMfaCodeMfaVerifyDto()
        {
            var dto = CreateValidMfaVerifyDto();
            dto.Code = "1234567";
            return dto;
        }

        private static MfaVerifyDto CreateNonNumericMfaCodeMfaVerifyDto()
        {
            var dto = CreateValidMfaVerifyDto();
            dto.Code = "abcdef";
            return dto;
        }

        #endregion
    }

    /// <summary>
    /// Fluent builder for CustomerDto objects
    /// </summary>
    public class CustomerDtoBuilder
    {
        private readonly CustomerDto _customerDto;

        public CustomerDtoBuilder()
        {
            _customerDto = new CustomerDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = "1234567890123456",
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = "123456789",
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        public CustomerDtoBuilder WithId(int id)
        {
            _customerDto.Id = id;
            return this;
        }

        public CustomerDtoBuilder WithName(string firstName, string lastName)
        {
            _customerDto.FirstName = firstName;
            _customerDto.LastName = lastName;
            return this;
        }

        public CustomerDtoBuilder WithEmail(string email)
        {
            _customerDto.Email = email;
            return this;
        }

        public CustomerDtoBuilder WithPhone(string phoneNumber)
        {
            _customerDto.PhoneNumber = phoneNumber;
            return this;
        }

        public CustomerDtoBuilder WithAccountNumber(string accountNumber)
        {
            _customerDto.AccountNumber = accountNumber;
            return this;
        }

        public CustomerDtoBuilder WithAddress(string address, string city, string state, string zipCode)
        {
            _customerDto.Address = address;
            _customerDto.City = city;
            _customerDto.State = state;
            _customerDto.ZipCode = zipCode;
            return this;
        }

        public CustomerDtoBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            _customerDto.DateOfBirth = dateOfBirth;
            return this;
        }

        public CustomerDtoBuilder WithSSN(string ssn)
        {
            _customerDto.Ssn = ssn;
            return this;
        }

        public CustomerDtoBuilder WithStatus(EnrollmentStatus status)
        {
            _customerDto.Status = status;
            return this;
        }

        public CustomerDtoBuilder WithCreatedAt(DateTime createdAt)
        {
            _customerDto.CreatedAt = createdAt;
            return this;
        }

        public CustomerDto Build() => _customerDto;
    }

    /// <summary>
    /// Fluent builder for Customer entities
    /// </summary>
    public class CustomerBuilder
    {
        private readonly Customer _customer;

        public CustomerBuilder()
        {
            _customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = "1234567890123456",
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = "123456789",
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        public CustomerBuilder WithId(int id)
        {
            _customer.Id = id;
            return this;
        }

        public CustomerBuilder WithName(string firstName, string lastName)
        {
            _customer.FirstName = firstName;
            _customer.LastName = lastName;
            return this;
        }

        public CustomerBuilder WithEmail(string email)
        {
            _customer.Email = email;
            return this;
        }

        public CustomerBuilder WithPhone(string phoneNumber)
        {
            _customer.PhoneNumber = phoneNumber;
            return this;
        }

        public CustomerBuilder WithAccountNumber(string accountNumber)
        {
            _customer.AccountNumber = accountNumber;
            return this;
        }

        public CustomerBuilder WithAddress(string address, string city, string state, string zipCode)
        {
            _customer.Address = address;
            _customer.City = city;
            _customer.State = state;
            _customer.ZipCode = zipCode;
            return this;
        }

        public CustomerBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            _customer.DateOfBirth = dateOfBirth;
            return this;
        }

        public CustomerBuilder WithSSN(string ssn)
        {
            _customer.Ssn = ssn;
            return this;
        }

        public CustomerBuilder WithStatus(EnrollmentStatus status)
        {
            _customer.Status = status;
            return this;
        }

        public CustomerBuilder WithCreatedAt(DateTime createdAt)
        {
            _customer.CreatedAt = createdAt;
            return this;
        }

        public Customer Build() => _customer;
    }
}
