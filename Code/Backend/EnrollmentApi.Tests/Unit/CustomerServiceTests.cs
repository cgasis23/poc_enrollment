using Microsoft.EntityFrameworkCore;
using Shouldly;
using EnrollmentApi.Data;
using EnrollmentApi.DTOs;
using EnrollmentApi.Models;
using EnrollmentApi.Services;

namespace EnrollmentApi.Tests.Unit
{
    public class CustomerServiceTests : IDisposable
    {
        private readonly EnrollmentDbContext _context;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            // Create in-memory database for testing
            var options = new DbContextOptionsBuilder<EnrollmentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new EnrollmentDbContext(options);
            _customerService = new CustomerService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithValidData_ShouldReturnCustomer()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "123456789";
            var birthdate = "1990-01-01";
            var expectedCustomer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = accountNumber,
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = ssn,
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(expectedCustomer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.LocateCustomerAsync(accountNumber, ssn, birthdate);

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
            var accountNumberWithSpaces = "1234 5678 9012 3456";
            var accountNumberWithoutSpaces = "1234567890123456";
            var ssn = "123456789";
            var birthdate = "1990-01-01";
            var expectedCustomer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = accountNumberWithoutSpaces,
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = ssn,
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(expectedCustomer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.LocateCustomerAsync(accountNumberWithSpaces, ssn, birthdate);

            // Assert
            result.ShouldNotBeNull();
            result!.FirstName.ShouldBe("John");
            result.LastName.ShouldBe("Doe");
            result.AccountNumber.ShouldBe(accountNumberWithoutSpaces);
        }

        [Fact]
        public async Task LocateCustomerAsync_WithInvalidAccountNumber_ShouldReturnNull()
        {
            // Arrange
            var accountNumber = "9999999999999999"; // Invalid account number
            var ssn = "123456789";
            var birthdate = "1990-01-01";
            var expectedCustomer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = "1234567890123456", // Different account number
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = ssn,
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(expectedCustomer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithInvalidSSN_ShouldReturnNull()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "999999999"; // Invalid SSN
            var birthdate = "1990-01-01";
            var expectedCustomer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = accountNumber,
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = "123456789", // Different SSN
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(expectedCustomer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithInvalidBirthdate_ShouldReturnNull()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "123456789";
            var birthdate = "1985-05-15"; // Different birthdate
            var expectedCustomer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = accountNumber,
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1), // Different birthdate
                Ssn = ssn,
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(expectedCustomer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithInvalidDateFormat_ShouldReturnNull()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "123456789";
            var birthdate = "invalid-date";
            var expectedCustomer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = accountNumber,
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = ssn,
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(expectedCustomer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithNoMatchingCustomer_ShouldReturnNull()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "123456789";
            var birthdate = "1990-01-01";

            // No customers in database

            // Act
            var result = await _customerService.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task LocateCustomerAsync_WithMultipleCustomers_ShouldReturnCorrectCustomer()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "123456789";
            var birthdate = "1990-01-01";
            var expectedCustomer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                AccountNumber = accountNumber,
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "90210",
                DateOfBirth = new DateTime(1990, 1, 1),
                Ssn = ssn,
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var otherCustomer = new Customer
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
            };

            _context.Customers.AddRange(expectedCustomer, otherCustomer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.LocateCustomerAsync(accountNumber, ssn, birthdate);

            // Assert
            result.ShouldNotBeNull();
            result!.Id.ShouldBe(1);
            result.FirstName.ShouldBe("John");
            result.LastName.ShouldBe("Doe");
            result.AccountNumber.ShouldBe(accountNumber);
            result.Ssn.ShouldBe(ssn);
        }
    }
}
