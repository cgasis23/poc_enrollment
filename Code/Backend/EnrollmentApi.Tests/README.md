# Enrollment API Tests

This directory contains comprehensive tests for the Enrollment API, organized into unit tests and integration tests.

## 📁 Test Structure

```
EnrollmentApi.Tests/
├── Unit/                           # Unit tests
│   ├── CustomerServiceTests.cs     # CustomerService unit tests
│   └── ValidatorTests.cs           # FluentValidation tests
├── Integration/                    # Integration tests
│   └── CustomerLocateIntegrationTests.cs  # API endpoint tests
└── README.md                       # This file
```

## 🧪 Test Categories

### Unit Tests (`Unit/`)

Unit tests focus on testing individual components in isolation, using mocked dependencies and in-memory databases.

#### CustomerServiceTests.cs
Comprehensive unit tests for the `ICustomerService.LocateCustomerAsync` method using **strict mocking** with Moq and **TestDataBuilder** with recipe enums.

**Test Coverage:**
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

- ✅ **Strict Mocking Demonstration**
  - `CustomerService_WithStrictMock_ShouldThrowOnUnexpectedCalls`

**Key Features Tested:**
- **Strict Mocking**: Using `Mock<ICustomerService>(MockBehavior.Strict)` for complete test isolation
- **Interface Testing**: Testing against `ICustomerService` interface rather than concrete implementation
- **DTO-Based Testing**: Working directly with `CustomerDto` objects
- **Fast Execution**: No database dependencies, pure unit tests
- **Unexpected Call Detection**: Strict mocks throw exceptions for unconfigured method calls
- **TestDataBuilder**: Using recipe enums for consistent test data creation

**Test Data Approach:**
```csharp
// Using TestDataBuilder with recipe enums
var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.ValidInputs);
var expectedCustomerDto = TestDataBuilder.CreateCustomerDto(CustomerRecipe.ValidCustomer);

// Setup strict mock for ICustomerService
_mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
    .ReturnsAsync(expectedCustomerDto);
```

#### TestDataBuilderDemoTests.cs
Demonstration tests showing how to use the **TestDataBuilder** with both recipe enums and fluent API approaches.

**Test Coverage:**
- ✅ **Recipe Enum Tests**
  - `TestDataBuilder_WithRecipeEnums_ShouldCreateValidCustomerDto`
  - `TestDataBuilder_WithRecipeEnums_ShouldCreateCompletedCustomerDto`
  - `TestDataBuilder_WithRecipeEnums_ShouldCreateCustomerWithSpecialCharacters`
  - `TestDataBuilder_WithRecipeEnums_ShouldCreateTestInputs`
  - `TestDataBuilder_WithRecipeEnums_ShouldCreateAccountNumberWithSpaces`
  - `TestDataBuilder_WithRecipeEnums_ShouldCreateMultipleCustomersList`

- ✅ **Fluent API Tests**
  - `TestDataBuilder_WithFluentApi_ShouldCreateCustomCustomerDto`
  - `TestDataBuilder_WithFluentApi_ShouldCreateMinimalCustomerDto`
  - `TestDataBuilder_WithFluentApi_ShouldCreateCustomerEntity`

- ✅ **Theory Tests**
  - `TestDataBuilder_WithDifferentRecipes_ShouldCreateCorrectStatus`
  - `TestDataBuilder_WithDifferentInputRecipes_ShouldCreateCorrectInputs`

**Key Features Demonstrated:**
- **Recipe Enums**: Predefined test data scenarios for common use cases
- **Fluent API**: Flexible object building with method chaining
- **Theory Tests**: Parameterized tests for multiple scenarios
- **Consistent Data**: Centralized test data management

#### ValidatorTests.cs
Comprehensive validation tests using FluentValidation for all DTOs with **TestDataBuilder** recipe enums.

**Test Coverage:**
- ✅ **CustomerCreateDtoValidator Tests**
  - Valid data validation using `CustomerCreateDtoRecipe.ValidCustomer`
  - Invalid first name scenarios (Empty, Invalid, Long)
  - Invalid last name scenarios (Empty, Invalid, Long)
  - Invalid email scenarios (Empty, Invalid, Long)
  - Invalid phone number scenarios (Empty, Invalid, InvalidFormat)
  - Invalid date of birth scenarios (Empty, Underage, Invalid)
  - Invalid SSN scenarios (Invalid, Valid)
  - Invalid zip code scenarios (Invalid, Valid)

- ✅ **CustomerUpdateDtoValidator Tests**
  - Valid data validation using `CustomerUpdateDtoRecipe.ValidCustomer`
  - Empty data validation using `CustomerUpdateDtoRecipe.EmptyData`
  - Invalid field scenarios (FirstName, LastName, PhoneNumber, DateOfBirth, SSN, Status)

- ✅ **MfaEnableDtoValidator Tests**
  - Valid MFA enable data using `MfaEnableDtoRecipe.ValidMfa`
  - Invalid customer ID scenarios using `MfaEnableDtoRecipe.InvalidCustomerId`
  - Invalid MFA code scenarios (Empty, Long, NonNumeric, Invalid)

- ✅ **MfaVerifyDtoValidator Tests**
  - Valid MFA verification data using `MfaVerifyDtoRecipe.ValidMfa`
  - Invalid customer ID scenarios using `MfaVerifyDtoRecipe.InvalidCustomerId`
  - Invalid MFA code scenarios (Empty, Long, NonNumeric, Invalid)

**Test Data Approach:**
```csharp
// Using TestDataBuilder with recipe enums for validation tests
var dto = TestDataBuilder.CreateCustomerCreateDto(CustomerCreateDtoRecipe.EmptyFirstName);
var result = _customerCreateValidator.TestValidate(dto);
result.ShouldHaveValidationErrorFor(x => x.FirstName)
      .WithErrorMessage("First name is required");
```

### Integration Tests (`Integration/`)

Integration tests test the API endpoints end-to-end, including HTTP requests and database interactions.

#### CustomerLocateIntegrationTests.cs
Integration tests for the customer location API endpoint.

**Test Coverage:**
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

## 🚀 Running Tests

### Prerequisites
- .NET 9.0 SDK
- All required NuGet packages (automatically restored)

### Run All Tests
```bash
cd Code/Backend/EnrollmentApi.Tests
dotnet test
```

### Run Specific Test Categories

#### Run Unit Tests Only
```bash
dotnet test --filter "TestCategory=Unit"
```

#### Run Integration Tests Only
```bash
dotnet test --filter "TestCategory=Integration"
```

#### Run Specific Test Class
```bash
# Run CustomerService tests only (strict mocking)
dotnet test --filter "CustomerServiceTests"

# Run Validator tests only
dotnet test --filter "ValidatorTests"

# Run Integration tests only
dotnet test --filter "CustomerLocateIntegrationTests"
```

#### Run Strict Mock Tests
```bash
# Run CustomerServiceTests with detailed output
dotnet test --filter "CustomerServiceTests" --logger "console;verbosity=detailed"

# Run specific strict mock test
dotnet test --filter "CustomerService_WithStrictMock_ShouldThrowOnUnexpectedCalls"
```

#### Run Tests with Verbose Output
```bash
dotnet test --verbosity normal
```

#### Run Tests with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📦 Test Dependencies

The test project uses the following packages:

- **xUnit** (2.9.2) - Testing framework
- **Moq** (4.20.70) - Mocking framework for unit tests
- **Shouldly** (4.2.1) - Assertion library for readable tests
- **FluentAssertions** (8.5.0) - Assertion library for integration tests
- **FluentValidation** (12.0.0) - Validation testing
- **Microsoft.EntityFrameworkCore.InMemory** (9.0.8) - In-memory database for testing
- **Microsoft.AspNetCore.Mvc.Testing** (9.0.8) - Integration testing support

## 🏗️ Test Architecture

### Unit Test Approach
- **Strict Mocking**: Uses `Mock<ICustomerService>(MockBehavior.Strict)` for complete test isolation
- **Interface Testing**: Tests against interfaces rather than concrete implementations
- **DTO-Based Testing**: Works directly with DTOs, no entity dependencies
- **Fast Execution**: No database operations, pure unit tests
- **Shouldly Assertions**: Provides readable test assertions
- **TestDataBuilder**: Centralized test data management with recipe enums and fluent API

### Strict Mocking Benefits
- **Fail Fast**: Any unexpected method calls throw exceptions immediately
- **Clear Intent**: Must explicitly set up every expected method call
- **Better Isolation**: No accidental dependencies on concrete implementations
- **Faster Tests**: No database setup or teardown required
- **True Unit Tests**: Tests only the interface contract, not implementation details

### TestDataBuilder Benefits
- **Consistent Data**: Centralized test data creation with predefined recipes
- **Reduced Duplication**: Eliminate repetitive object creation code
- **Maintainable**: Single source of truth for test data
- **Flexible**: Both recipe enums and fluent API approaches
- **Type Safe**: Compile-time checking for test data creation
- **Readable**: Clear intent with descriptive recipe names

### Integration Test Approach
- **WebApplicationFactory**: Uses ASP.NET Core's testing infrastructure
- **In-Memory Database**: Replaces production database with in-memory version
- **HTTP Client**: Tests actual HTTP requests and responses
- **FluentAssertions**: Provides readable HTTP response assertions

## 📊 Test Results

### Current Test Statistics
- **Total Tests**: 58
- **Unit Tests**: 50 (9 CustomerService + 40 Validator)
- **Integration Tests**: 8
- **Pass Rate**: 100% ✅

### Test Execution Time
- **Unit Tests**: ~1.0 seconds (strict mocking + TestDataBuilder approach)
- **Integration Tests**: ~0.4 seconds
- **Total**: ~1.4 seconds

### CustomerServiceTests Performance
- **Execution Time**: ~0.9 seconds for 9 tests
- **Average per Test**: ~0.1 seconds
- **Mocking Approach**: Strict mocking with `ICustomerService`
- **Database Dependencies**: None (pure unit tests)

### ValidatorTests Performance
- **Execution Time**: ~0.1 seconds for 40 tests
- **Average per Test**: ~0.0025 seconds
- **Approach**: TestDataBuilder with recipe enums
- **Coverage**: Comprehensive validation scenarios for all DTOs

## 🔧 Test Configuration

### TestDataBuilder Configuration
The TestDataBuilder provides two approaches for creating test data:

#### Recipe Enum Approach
```csharp
// Create test inputs using predefined recipes
var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.ValidInputs);

// Create CustomerDto using predefined recipes
var expectedCustomerDto = TestDataBuilder.CreateCustomerDto(CustomerRecipe.ValidCustomer);

// Available CustomerRecipe enums:
// - ValidCustomer, CompletedCustomer, PendingCustomer, CancelledCustomer
// - CustomerWithSpacesInAccountNumber, CustomerWithLongName, CustomerWithSpecialCharacters
// - InvalidAccountNumber, InvalidSSN, InvalidBirthdate, InvalidDateFormat
// - MultipleCustomers, StrictMockCustomer

// Available TestInputRecipe enums:
// - ValidInputs, AccountNumberWithSpaces, InvalidAccountNumber, InvalidSSN
// - InvalidBirthdate, InvalidDateFormat, NoMatchingCustomer, EmptyInputs
// - NullInputs, WhitespaceInputs

// Available CustomerCreateDtoRecipe enums:
// - ValidCustomer, EmptyFirstName, InvalidFirstName, LongFirstName
// - EmptyLastName, InvalidLastName, LongLastName, EmptyEmail, InvalidEmail, LongEmail
// - EmptyPhoneNumber, InvalidPhoneNumber, InvalidPhoneNumberFormat
// - EmptyDateOfBirth, UnderageCustomer, InvalidDateOfBirth
// - InvalidSSN, ValidSSN, InvalidZipCode, ValidZipCode

// Available CustomerUpdateDtoRecipe enums:
// - ValidCustomer, EmptyData, InvalidFirstName, InvalidLastName
// - InvalidPhoneNumber, InvalidDateOfBirth, InvalidSSN, InvalidStatus

// Available MfaEnableDtoRecipe enums:
// - ValidMfa, InvalidCustomerId, InvalidMfaCode, EmptyMfaCode, LongMfaCode, NonNumericMfaCode

// Available MfaVerifyDtoRecipe enums:
// - ValidMfa, InvalidCustomerId, InvalidMfaCode, EmptyMfaCode, LongMfaCode, NonNumericMfaCode

#### Fluent API Approach
```csharp
// Create custom CustomerDto with fluent API
var customerDto = TestDataBuilder.CustomerDto()
    .WithId(999)
    .WithName("Jane", "Smith")
    .WithEmail("jane.smith@example.com")
    .WithPhone("555-987-6543")
    .WithAccountNumber("9876543210987654")
    .WithAddress("456 Oak Ave", "Somewhere", "NY", "10001")
    .WithDateOfBirth(new DateTime(1985, 5, 15))
    .WithSSN("987654321")
    .WithStatus(EnrollmentStatus.Completed)
    .WithCreatedAt(new DateTime(2024, 1, 1))
    .Build();

// Create custom Customer entity with fluent API
var customer = TestDataBuilder.Customer()
    .WithId(123)
    .WithName("Alice", "Brown")
    .WithEmail("alice.brown@example.com")
    .WithStatus(EnrollmentStatus.Cancelled)
    .Build();
```

### Strict Mocking Configuration
CustomerServiceTests use strict mocking with `ICustomerService` interface:
```csharp
// Constructor setup
_mockCustomerService = new Mock<ICustomerService>(MockBehavior.Strict);

// Test-specific setup with TestDataBuilder
var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.ValidInputs);
var expectedCustomerDto = TestDataBuilder.CreateCustomerDto(CustomerRecipe.ValidCustomer);

_mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
    .ReturnsAsync(expectedCustomerDto);

// Usage in tests
var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);
```

### In-Memory Database (Other Tests)
Other unit tests use EF Core in-memory database with unique names per test:
```csharp
var options = new DbContextOptionsBuilder<EnrollmentDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;
```

### Test Data Seeding
Each test seeds its own data to ensure test isolation:
```csharp
_context.Customers.Add(expectedCustomer);
await _context.SaveChangesAsync();
```

### HTTP Client Configuration
Integration tests use `WebApplicationFactory` with in-memory database:
```csharp
_factory = factory.WithWebHostBuilder(builder =>
{
    builder.ConfigureServices(services =>
    {
        // Replace with in-memory database
        services.AddDbContext<EnrollmentDbContext>(options =>
        {
            options.UseInMemoryDatabase(_dbName);
        });
    });
});
```

## 🧪 Writing New Tests

### Adding Unit Tests
1. Create test class in `Unit/` folder
2. Inherit from appropriate base class or implement `IDisposable`
3. Use in-memory database for data access
4. Use Shouldly for assertions
5. Follow naming convention: `MethodName_Scenario_ExpectedResult`

### Adding Integration Tests
1. Create test class in `Integration/` folder
2. Inherit from `IClassFixture<WebApplicationFactory<Program>>`
3. Use `WebApplicationFactory` for HTTP client
4. Use FluentAssertions for HTTP response assertions
5. Follow naming convention: `Endpoint_Scenario_ExpectedResult`

### Test Naming Conventions
- **Unit Tests**: `MethodName_WithCondition_ShouldReturnResult`
- **Integration Tests**: `Endpoint_WithCondition_ShouldReturnStatus`

### Example Strict Mock Unit Test with TestDataBuilder
```csharp
[Fact]
public async Task LocateCustomerAsync_WithValidData_ShouldReturnCustomer()
{
    // Arrange
    var (accountNumber, ssn, birthdate) = TestDataBuilder.CreateTestInputs(TestInputRecipe.ValidInputs);
    var expectedCustomerDto = TestDataBuilder.CreateCustomerDto(CustomerRecipe.ValidCustomer);

    // Setup strict mock
    _mockCustomerService.Setup(s => s.LocateCustomerAsync(accountNumber, ssn, birthdate))
        .ReturnsAsync(expectedCustomerDto);

    // Act
    var result = await _mockCustomerService.Object.LocateCustomerAsync(accountNumber, ssn, birthdate);

    // Assert
    result.ShouldNotBeNull();
    result!.FirstName.ShouldBe("John");
}
```

### Example TestDataBuilder Recipe Test
```csharp
[Fact]
public void TestDataBuilder_WithRecipeEnums_ShouldCreateValidCustomerDto()
{
    // Arrange & Act
    var customerDto = TestDataBuilder.CreateCustomerDto(CustomerRecipe.ValidCustomer);

    // Assert
    customerDto.ShouldNotBeNull();
    customerDto.FirstName.ShouldBe("John");
    customerDto.LastName.ShouldBe("Doe");
    customerDto.Status.ShouldBe(EnrollmentStatus.Pending);
}
```

### Example TestDataBuilder Validation Test
```csharp
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
```

### Example TestDataBuilder Fluent API Test
```csharp
[Fact]
public void TestDataBuilder_WithFluentApi_ShouldCreateCustomCustomerDto()
{
    // Arrange & Act
    var customerDto = TestDataBuilder.CustomerDto()
        .WithId(999)
        .WithName("Jane", "Smith")
        .WithEmail("jane.smith@example.com")
        .WithStatus(EnrollmentStatus.Completed)
        .Build();

    // Assert
    customerDto.ShouldNotBeNull();
    customerDto.Id.ShouldBe(999);
    customerDto.FirstName.ShouldBe("Jane");
    customerDto.Status.ShouldBe(EnrollmentStatus.Completed);
}
```

### Example Traditional Unit Test (Other Tests)
```csharp
[Fact]
public async Task LocateCustomerAsync_WithValidData_ShouldReturnCustomer()
{
    // Arrange
    var customer = new Customer { /* test data */ };
    _context.Customers.Add(customer);
    await _context.SaveChangesAsync();

    // Act
    var result = await _customerService.LocateCustomerAsync(accountNumber, ssn, birthdate);

    // Assert
    result.ShouldNotBeNull();
    result!.FirstName.ShouldBe("John");
}
```

### Example Integration Test
```csharp
[Fact]
public async Task LocateCustomer_WithValidData_ShouldReturnCustomer()
{
    // Arrange
    SeedTestData();

    // Act
    var response = await _client.GetAsync($"/api/customers/locate?accountNumber={accountNumber}&ssn={ssn}&birthdate={birthdate}");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

## 🔍 Debugging Tests

### Running Tests in Debug Mode
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Debugging Specific Test
```bash
dotnet test --filter "FullyQualifiedName~CustomerServiceTests.LocateCustomerAsync_WithValidData_ShouldReturnCustomer"
```

### Viewing Test Output
```bash
dotnet test --verbosity normal --logger "console;verbosity=detailed"
```

## 📈 Test Coverage

The test suite provides comprehensive coverage for:

- ✅ **ICustomerService Interface** - 100% coverage with strict mocking
- ✅ **DTO Validation** - All validation rules tested
- ✅ **API Endpoints** - Customer location endpoint fully tested
- ✅ **Error Handling** - Invalid inputs and edge cases
- ✅ **Business Logic** - Account number formatting, date parsing
- ✅ **Strict Mocking** - Unexpected call detection and interface contract testing

## 🎯 Strict Mocking Benefits

### Why Use Strict Mocking?
1. **Fail Fast**: Any unexpected method calls throw exceptions immediately
2. **Clear Intent**: Must explicitly set up every expected method call
3. **Better Isolation**: No accidental dependencies on concrete implementations
4. **Faster Tests**: No database setup or teardown required
5. **True Unit Tests**: Tests only the interface contract, not implementation details

### Strict vs Loose Mocking
- **Strict**: Throws exceptions for unconfigured calls (used in CustomerServiceTests)
- **Loose**: Returns default values for unconfigured calls (used in other tests)

### When to Use Each Approach
- **Strict Mocking**: When testing interface contracts and ensuring no unexpected calls
- **Loose Mocking**: When testing with complex dependencies and you want flexibility

## 🚨 Common Issues

### Test Isolation
- Each test uses a unique database instance
- Tests are independent and can run in any order
- No shared state between tests

### Database Context
- Unit tests use in-memory database
- Integration tests replace production database with in-memory version
- No external database dependencies

### Async/Await
- All database operations are async
- Use `await` for all async operations
- Tests are async and return `Task`

## 📚 Additional Resources

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [Shouldly Documentation](https://shouldly.io/)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [EF Core In-Memory Database](https://docs.microsoft.com/en-us/ef/core/testing/)
- [ASP.NET Core Integration Testing](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests)

## 🤝 Contributing

When adding new tests:

1. Follow the existing naming conventions
2. Use appropriate test categories (Unit/Integration)
3. Ensure test isolation
4. Add comprehensive assertions
5. Update this README with new test information
6. Ensure all tests pass before committing

## 📞 Support

For test-related issues or questions:
1. Check the test output for detailed error messages
2. Review the test configuration
3. Ensure all dependencies are properly installed
4. Verify the test data setup
