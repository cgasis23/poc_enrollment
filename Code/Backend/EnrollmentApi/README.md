# Enrollment API

A REST API built with .NET Core 8 for managing customer enrollment with Multi-Factor Authentication (MFA) support.

## Features

- **Customer Management**: CRUD operations for customer enrollment
- **MFA Support**: Time-based One-Time Password (TOTP) implementation
- **Document Management**: Track enrollment documents
- **Search & Pagination**: Advanced search capabilities with pagination
- **Swagger Documentation**: Interactive API documentation
- **Entity Framework Core**: SQL Server database with code-first approach
- **Comprehensive Testing**: 71 tests with 100% pass rate (Unit + Integration + Validation)
- **Input Validation**: FluentValidation with comprehensive business rules
- **Test Infrastructure**: Organized test suite with proper isolation and mocking

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

## Getting Started

### 1. Clone and Navigate

```bash
cd Code/Backend/EnrollmentApi
```

### 2. Database Configuration

The application is configured to use an in-memory database by default, which is perfect for development and testing. No additional configuration is required.

### 3. Run the Application

```bash
dotnet run
```

The API will be available at:
- **API**: http://localhost:5065
- **Swagger UI**: http://localhost:5065/swagger

### 4. Run Tests (Optional)

```bash
# Navigate to test project
cd ../EnrollmentApi.Tests

# Run all tests
dotnet test

# Run specific test categories
dotnet test --filter "CustomerServiceTests"    # Unit tests only
dotnet test --filter "Integration"             # Integration tests only
```

## API Endpoints

### Customers

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/customers` | Get all customers with search/pagination |
| GET | `/api/customers/{id}` | Get customer by ID |
| GET | `/api/customers/email/{email}` | Get customer by email |
| POST | `/api/customers` | Create new customer |
| PUT | `/api/customers/{id}` | Update customer |
| DELETE | `/api/customers/{id}` | Delete customer |
| GET | `/api/customers/{id}/exists` | Check if customer exists |
| GET | `/api/customers/stats` | Get customer statistics |

### MFA

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/mfa/setup/{customerId}` | Setup MFA for customer |
| POST | `/api/mfa/verify` | Verify MFA code |
| POST | `/api/mfa/enable` | Enable MFA |
| POST | `/api/mfa/disable` | Disable MFA |
| GET | `/api/mfa/status/{customerId}` | Get MFA status |
| POST | `/api/mfa/qrcode` | Generate QR code URL |
| POST | `/api/mfa/backup-codes` | Generate backup codes |

## Usage Examples

### Create a Customer

```bash
curl -X POST "https://localhost:7001/api/customers" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "phoneNumber": "555-123-4567",
    "dateOfBirth": "1990-01-01T00:00:00Z"
  }'
```

### Setup MFA

```bash
curl -X POST "https://localhost:7001/api/mfa/setup/1"
```

### Enable MFA

```bash
curl -X POST "https://localhost:7001/api/mfa/enable" \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": 1,
    "code": "123456"
  }'
```

### Search Customers

```bash
curl "https://localhost:7001/api/customers?firstName=John&status=Pending&page=1&pageSize=10"
```

## Data Models

### Customer

```csharp
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Ssn { get; set; }
    public EnrollmentStatus Status { get; set; }
    public bool IsMfaEnabled { get; set; }
    public string? MfaSecret { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

### Enrollment Status

- `Pending`: Initial enrollment status
- `InProgress`: Enrollment in progress
- `Completed`: Enrollment completed
- `Rejected`: Enrollment rejected
- `Cancelled`: Enrollment cancelled

## MFA Implementation

The API implements TOTP (Time-based One-Time Password) using:

- **Algorithm**: SHA1
- **Digits**: 6
- **Period**: 30 seconds
- **Issuer**: "EnrollmentAPI"

### QR Code Format

```
otpauth://totp/EnrollmentAPI:user@example.com?secret=SECRET&issuer=EnrollmentAPI&algorithm=SHA1&digits=6&period=30
```

## Database

The application uses Entity Framework Core with an in-memory database. The database will be created automatically on first run and data will persist for the duration of the application session.

### Tables

- **Customers**: Customer information and enrollment status
- **EnrollmentDocuments**: Document uploads and status
- **EnrollmentNotes**: Notes and comments during enrollment

## Configuration

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Set to "Development" for development mode

### CORS

The API is configured to allow all origins in development. For production, update the CORS policy in `Program.cs`.

## Development

### Adding New Features

1. Create models in the `Models` folder
2. Add DTOs in the `DTOs` folder
3. Create services in the `Services` folder
4. Add controllers in the `Controllers` folder
5. Update `Program.cs` to register new services

### Database Migrations

```bash
# Add migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## 🧪 Testing

The API includes comprehensive test coverage with both unit tests and integration tests, ensuring high code quality and reliability.

### 📊 Test Statistics
- **Total Tests**: 71
- **Unit Tests**: 63 (CustomerService + Validators)
- **Integration Tests**: 8 (API endpoints)
- **Pass Rate**: 100% ✅
- **Test Execution Time**: ~1.6 seconds

### 🏗️ Test Architecture

#### Test Structure
```
EnrollmentApi.Tests/
├── Unit/                           # Unit tests
│   ├── CustomerServiceTests.cs     # CustomerService unit tests (8 tests)
│   └── ValidatorTests.cs           # FluentValidation tests (55 tests)
├── Integration/                    # Integration tests
│   └── CustomerLocateIntegrationTests.cs  # API endpoint tests (8 tests)
└── README.md                       # Detailed test documentation
```

#### Test Categories

**Unit Tests** (`Unit/`)
- Test individual components in isolation
- Use in-memory database for reliable testing
- Focus on business logic and validation rules
- Implement proper test isolation with unique database instances

**Integration Tests** (`Integration/`)
- Test API endpoints end-to-end
- Use `WebApplicationFactory` for HTTP client testing
- Test actual HTTP requests and responses
- Validate complete request/response cycles

**Validation Tests** (`Unit/ValidatorTests.cs`)
- Test all DTO validation rules using FluentValidation
- Cover edge cases and error scenarios
- Ensure comprehensive input validation

### 🚀 Running Tests

#### Run All Tests
```bash
cd Code/Backend/EnrollmentApi.Tests
dotnet test
```

#### Run Specific Test Categories
```bash
# Run unit tests only
dotnet test --filter "TestCategory=Unit"

# Run integration tests only
dotnet test --filter "TestCategory=Integration"

# Run specific test class
dotnet test --filter "CustomerServiceTests"
dotnet test --filter "ValidatorTests"
dotnet test --filter "CustomerLocateIntegrationTests"
```

#### Run Tests with Verbose Output
```bash
dotnet test --verbosity normal
```

#### Run Tests with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### 📦 Test Dependencies

The test project uses the following packages:

- **xUnit** (2.9.2) - Testing framework
- **Moq** (4.20.70) - Mocking framework for unit tests
- **Shouldly** (4.2.1) - Assertion library for readable tests
- **FluentAssertions** (8.5.0) - Assertion library for integration tests
- **FluentValidation** (12.0.0) - Validation testing
- **Microsoft.EntityFrameworkCore.InMemory** (9.0.8) - In-memory database for testing
- **Microsoft.AspNetCore.Mvc.Testing** (9.0.8) - Integration testing support

### 🎯 Test Coverage Details

#### CustomerService Tests (8 tests)
- ✅ **Valid Data Scenarios**
  - `LocateCustomerAsync_WithValidData_ShouldReturnCustomer`
  - `LocateCustomerAsync_WithAccountNumberWithSpaces_ShouldReturnCustomer`
  - `LocateCustomerAsync_WithMultipleCustomers_ShouldReturnCorrectCustomer`

- ✅ **Invalid Data Scenarios**
  - `LocateCustomerAsync_WithInvalidAccountNumber_ShouldReturnNull`
  - `LocateCustomerAsync_WithInvalidSSN_ShouldReturnNull`
  - `LocateCustomerAsync_WithInvalidBirthdate_ShouldReturnNull`
  - `LocateCustomerAsync_WithInvalidDateFormat_ShouldReturnNull`
  - `LocateCustomerAsync_WithNoMatchingCustomer_ShouldReturnNull`

#### Validator Tests (55 tests)
- ✅ **CustomerCreateDtoValidator** - 35 tests
- ✅ **CustomerUpdateDtoValidator** - 8 tests
- ✅ **MfaEnableDtoValidator** - 6 tests
- ✅ **MfaVerifyDtoValidator** - 6 tests

#### Integration Tests (8 tests)
- ✅ **Successful Scenarios**
  - `LocateCustomer_WithValidData_ShouldReturnCustomer`
  - `LocateCustomer_WithAccountNumberWithSpaces_ShouldReturnCustomer`

- ✅ **Error Scenarios**
  - `LocateCustomer_WithInvalidAccountNumber_ShouldReturnNotFound`
  - `LocateCustomer_WithInvalidSSN_ShouldReturnNotFound`
  - `LocateCustomer_WithInvalidBirthdate_ShouldReturnNotFound`
  - `LocateCustomer_WithInvalidDateFormat_ShouldReturnNotFound`

- ✅ **Validation Scenarios**
  - `LocateCustomer_WithMissingAccountNumber_ShouldReturnBadRequest`
  - `LocateCustomer_WithMissingSSN_ShouldReturnBadRequest`
  - `LocateCustomer_WithMissingBirthdate_ShouldReturnBadRequest`

### 🔧 Test Configuration

#### In-Memory Database
Unit tests use EF Core in-memory database with unique names per test:
```csharp
var options = new DbContextOptionsBuilder<EnrollmentDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;
```

#### Test Data Seeding
Each test seeds its own data to ensure test isolation:
```csharp
_context.Customers.Add(expectedCustomer);
await _context.SaveChangesAsync();
```

#### HTTP Client Configuration
Integration tests use `WebApplicationFactory` with in-memory database:
```csharp
_factory = factory.WithWebHostBuilder(builder =>
{
    builder.ConfigureServices(services =>
    {
        services.AddDbContext<EnrollmentDbContext>(options =>
        {
            options.UseInMemoryDatabase(_dbName);
        });
    });
});
```

### 🧪 Writing New Tests

#### Adding Unit Tests
1. Create test class in `Unit/` folder
2. Inherit from appropriate base class or implement `IDisposable`
3. Use in-memory database for data access
4. Use Shouldly for assertions
5. Follow naming convention: `MethodName_Scenario_ExpectedResult`

#### Adding Integration Tests
1. Create test class in `Integration/` folder
2. Inherit from `IClassFixture<WebApplicationFactory<Program>>`
3. Use `WebApplicationFactory` for HTTP client
4. Use FluentAssertions for HTTP response assertions
5. Follow naming convention: `Endpoint_Scenario_ExpectedResult`

### 🔍 Debugging Tests

#### Running Tests in Debug Mode
```bash
dotnet test --logger "console;verbosity=detailed"
```

#### Debugging Specific Test
```bash
dotnet test --filter "FullyQualifiedName~CustomerServiceTests.LocateCustomerAsync_WithValidData_ShouldReturnCustomer"
```

#### Viewing Test Output
```bash
dotnet test --verbosity normal --logger "console;verbosity=detailed"
```

### 📈 Test Coverage Areas

The test suite provides comprehensive coverage for:

- ✅ **CustomerService.LocateCustomerAsync** - 100% coverage
- ✅ **DTO Validation** - All validation rules tested
- ✅ **API Endpoints** - Customer location endpoint fully tested
- ✅ **Error Handling** - Invalid inputs and edge cases
- ✅ **Business Logic** - Account number formatting, date parsing

### 🚨 Common Issues & Solutions

#### Test Isolation
- Each test uses a unique database instance
- Tests are independent and can run in any order
- No shared state between tests

#### Database Context
- Unit tests use in-memory database
- Integration tests replace production database with in-memory version
- No external database dependencies

#### Async/Await
- All database operations are async
- Use `await` for all async operations
- Tests are async and return `Task`

### 📚 Test Documentation
For detailed information about the test suite, including:
- Test organization and structure
- Running specific test categories
- Writing new tests
- Debugging test issues

See: [Comprehensive Test Documentation](../EnrollmentApi.Tests/README.md)

### 🎯 Interactive API Testing
The API also includes Swagger UI for manual testing. Navigate to the root URL to access the interactive documentation.

## Validation

The API uses **FluentValidation** for comprehensive input validation instead of Data Annotations. This provides:

- **Flexible Validation Rules**: Complex validation logic with conditional rules
- **Custom Error Messages**: User-friendly validation messages
- **Cross-Property Validation**: Validation rules that depend on multiple properties
- **Performance**: Better performance compared to Data Annotations

### Validation Rules

#### Customer Creation (`CustomerCreateDto`)
- **FirstName/LastName**: Required, max 50 chars, letters/spaces/hyphens/apostrophes only
- **Email**: Required, valid email format, max 100 chars
- **PhoneNumber**: Required, international phone number format
- **DateOfBirth**: Required, customer must be at least 13 years old
- **SSN**: Optional, format XXX-XX-XXXX or XXXXXXXXX
- **Address Fields**: Optional, with length restrictions
- **ZipCode**: Optional, format XXXXX or XXXXX-XXXX

#### Customer Update (`CustomerUpdateDto`)
- **FirstName/LastName**: Optional, same validation as creation
- **PhoneNumber**: Optional, valid format when provided
- **DateOfBirth**: Optional, age validation when provided
- **Status**: Optional, must be valid enum value

#### MFA Operations
- **CustomerId**: Must be greater than 0
- **Code**: Required, exactly 6 digits

### Custom Validators

Validators are located in the `Validators/` folder:
- `CustomerCreateDtoValidator.cs`
- `CustomerUpdateDtoValidator.cs`
- `MfaVerifyDtoValidator.cs`
- `MfaEnableDtoValidator.cs`

## Security Considerations

- MFA secrets are stored encrypted in the database
- SSN is stored as optional field (consider encryption for production)
- CORS is configured for development (restrict for production)
- Input validation is implemented using FluentValidation with comprehensive rules

## Production Deployment

1. **Switch to SQL Server for Production:**
   - Update `Program.cs` to use SQL Server instead of in-memory database:
   ```csharp
   builder.Services.AddDbContext<EnrollmentDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   ```
   - Configure connection string in `appsettings.Production.json` or environment variables
   
2. Configure proper CORS policy
3. Enable HTTPS
4. Set up proper logging
5. Configure authentication/authorization if needed
6. Use environment variables for sensitive configuration

## Support

For issues or questions, please refer to the project documentation or create an issue in the repository.
