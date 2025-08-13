using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using EnrollmentApi.Data;
using EnrollmentApi.Models;
using System.Net;
using System.Text.Json;

namespace EnrollmentApi.Tests.Integration
{
    public class CustomerLocateIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly string _dbName = "TestDb_CustomerLocate";

        public CustomerLocateIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<EnrollmentDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add in-memory database for testing
                    services.AddDbContext<EnrollmentDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(_dbName);
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        private void SeedTestData(EnrollmentDbContext context)
        {
            // Clear existing data
            context.Customers.RemoveRange(context.Customers);
            context.SaveChanges();

            // Add test customer
            var testCustomer = new Customer
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

            context.Customers.Add(testCustomer);
            context.SaveChanges();
        }

        private void EnsureTestDataSeeded()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EnrollmentDbContext>();
            db.Database.EnsureCreated();
            SeedTestData(db);
        }

        [Fact]
        public async Task LocateCustomer_WithValidData_ShouldReturnCustomer()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "123456789";
            var birthdate = "1990-01-01";

            // Seed test data
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EnrollmentDbContext>();
            db.Database.EnsureCreated();
            SeedTestData(db);

            // Act
            var response = await _client.GetAsync($"/api/customers/locate?accountNumber={accountNumber}&ssn={ssn}&birthdate={birthdate}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var customer = JsonSerializer.Deserialize<CustomerDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            customer.Should().NotBeNull();
            customer!.FirstName.Should().Be("John");
            customer.LastName.Should().Be("Doe");
            customer.AccountNumber.Should().Be(accountNumber);
            customer.Ssn.Should().Be(ssn);
        }

        [Fact]
        public async Task LocateCustomer_WithInvalidAccountNumber_ShouldReturnNotFound()
        {
            // Arrange
            var accountNumber = "9999999999999999"; // Invalid account number
            var ssn = "123456789";
            var birthdate = "1990-01-01";

            // Seed test data
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EnrollmentDbContext>();
            db.Database.EnsureCreated();
            SeedTestData(db);

            // Act
            var response = await _client.GetAsync($"/api/customers/locate?accountNumber={accountNumber}&ssn={ssn}&birthdate={birthdate}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LocateCustomer_WithInvalidSSN_ShouldReturnNotFound()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "999999999"; // Invalid SSN
            var birthdate = "1990-01-01";

            // Seed test data
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EnrollmentDbContext>();
            db.Database.EnsureCreated();
            SeedTestData(db);

            // Act
            var response = await _client.GetAsync($"/api/customers/locate?accountNumber={accountNumber}&ssn={ssn}&birthdate={birthdate}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LocateCustomer_WithInvalidBirthdate_ShouldReturnNotFound()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "123456789";
            var birthdate = "1985-05-15"; // Different birthdate

            // Seed test data
            EnsureTestDataSeeded();

            // Act
            var response = await _client.GetAsync($"/api/customers/locate?accountNumber={accountNumber}&ssn={ssn}&birthdate={birthdate}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LocateCustomer_WithMissingAccountNumber_ShouldReturnBadRequest()
        {
            // Arrange
            var ssn = "123456789";
            var birthdate = "1990-01-01";

            // Act
            var response = await _client.GetAsync($"/api/customers/locate?ssn={ssn}&birthdate={birthdate}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LocateCustomer_WithMissingSSN_ShouldReturnBadRequest()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var birthdate = "1990-01-01";

            // Act
            var response = await _client.GetAsync($"/api/customers/locate?accountNumber={accountNumber}&birthdate={birthdate}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LocateCustomer_WithMissingBirthdate_ShouldReturnBadRequest()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "123456789";

            // Act
            var response = await _client.GetAsync($"/api/customers/locate?accountNumber={accountNumber}&ssn={ssn}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LocateCustomer_WithInvalidDateFormat_ShouldReturnNotFound()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var ssn = "123456789";
            var birthdate = "invalid-date";

            // Seed test data
            EnsureTestDataSeeded();

            // Act
            var response = await _client.GetAsync($"/api/customers/locate?accountNumber={accountNumber}&ssn={ssn}&birthdate={birthdate}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LocateCustomer_WithAccountNumberWithSpaces_ShouldReturnCustomer()
        {
            // Arrange
            var accountNumber = "1234 5678 9012 3456"; // With spaces
            var ssn = "123456789";
            var birthdate = "1990-01-01";

            // Seed test data
            EnsureTestDataSeeded();

            // Act
            var response = await _client.GetAsync($"/api/customers/locate?accountNumber={accountNumber}&ssn={ssn}&birthdate={birthdate}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var customer = JsonSerializer.Deserialize<CustomerDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            customer.Should().NotBeNull();
            customer!.FirstName.Should().Be("John");
            customer.LastName.Should().Be("Doe");
        }
    }

    // DTO class for deserialization
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? AccountNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Ssn { get; set; }
        public int Status { get; set; } // Changed from string to int to match the API response
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsMfaEnabled { get; set; }
        public DateTime? MfaEnabledAt { get; set; }
    }
}
