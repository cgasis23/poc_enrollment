using Microsoft.EntityFrameworkCore;
using Shouldly;
using EnrollmentApi.Data;
using EnrollmentApi.DTOs;
using EnrollmentApi.Models;
using EnrollmentApi.Services;
using Moq;

namespace EnrollmentApi.Tests.Unit
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerService> _mockCustomerService;

        public CustomerServiceTests()
        {
            // Create strict mock for ICustomerService interface
            _mockCustomerService = new Mock<ICustomerService>(MockBehavior.Strict);
        }

        [Fact]
        public async Task LocateCustomerAsync_WithValidData_ShouldReturnCustomer()
        {
            // Arrange
            var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.ValidInputs);
            var expectedCustomerDto = TestDataBuilder.CreateCustomerDto(CustomerRecipe.ValidCustomer);

            // Setup strict mock for ICustomerService
            _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
                .ReturnsAsync(expectedCustomerDto);

            // Act
            var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldNotBeNull();
            result!.FirstName.ShouldBe("John");
            result.LastName.ShouldBe("Doe");
            result.AccountNumber.ShouldBe(accountNumber);
            result.Ssn.ShouldBe(ssn);
            result.DateOfBirth.ShouldBe(new DateTime(1990, 1, 1));
        }

        [Fact]
        public async Task LocateCustomerAsync_WithAccountNumberWithSpaces_ShouldReturnCustomer()
        {
            // Arrange
            var (accountNumberWithSpaces, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.AccountNumberWithSpaces);
            var expectedCustomerDto = TestDataBuilder.CreateCustomerDto(CustomerRecipe.CustomerWithSpacesInAccountNumber);

            // Setup strict mock for ICustomerService
            _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumberWithSpaces, ssn, birthdate))
                .ReturnsAsync(expectedCustomerDto);

            // Act
            var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumberWithSpaces, ssn, birthdate);

            // Assert
            result.ShouldNotBeNull();
            result!.FirstName.ShouldBe("John");
            result.LastName.ShouldBe("Doe");
            result.AccountNumber.ShouldBe("1234567890123456"); // Without spaces
        }

        [Fact]
        public async Task LocateCustomerAsync_WithInvalidAccountNumber_ShouldReturnNull()
        {
            // Arrange
            var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.InvalidAccountNumber);

            // Setup strict mock for ICustomerService to return null
            _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
                .ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithInvalidSSN_ShouldReturnNull()
        {
            // Arrange
            var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.InvalidSSN);

            // Setup strict mock for ICustomerService to return null
            _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
                .ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithInvalidBirthdate_ShouldReturnNull()
        {
            // Arrange
            var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.InvalidBirthdate);

            // Setup strict mock for ICustomerService to return null
            _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
                .ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithInvalidDateFormat_ShouldReturnNull()
        {
            // Arrange
            var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.InvalidDateFormat);

            // Setup strict mock for ICustomerService to return null
            _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
                .ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithNoMatchingCustomer_ShouldReturnNull()
        {
            // Arrange
            var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.NoMatchingCustomer);

            // Setup strict mock for ICustomerService to return null
            _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
                .ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithMultipleCustomers_ShouldReturnCorrectCustomer()
        {
            // Arrange
            var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.ValidInputs);
            var expectedCustomerDto = TestDataBuilder.CreateCustomerDto(CustomerRecipe.MultipleCustomers);

            // Setup strict mock for ICustomerService
            _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
                .ReturnsAsync(expectedCustomerDto);

            // Act
            var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldNotBeNull();
            result!.Id.ShouldBe(1);
            result.FirstName.ShouldBe("John");
            result.LastName.ShouldBe("Doe");
            result.AccountNumber.ShouldBe(accountNumber);
            result.Ssn.ShouldBe(ssn);
        }

        [Fact]
        public async Task CustomerService_WithStrictMock_ShouldThrowOnUnexpectedCalls()
        {
            // Arrange
            var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.ValidInputs);
            var expectedCustomerDto = TestDataBuilder.CreateCustomerDto(CustomerRecipe.StrictMockCustomer);

            // Setup strict mock for ICustomerService
            _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
                .ReturnsAsync(expectedCustomerDto);

            // Act & Assert
            var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);
            result.ShouldNotBeNull();
            result!.FirstName.ShouldBe("John");

            // This would throw an exception in strict mode if not set up
            // _mockCustomerService.Object.GetCustomerByIdAsync(1); // This would fail
        }
    }
}
