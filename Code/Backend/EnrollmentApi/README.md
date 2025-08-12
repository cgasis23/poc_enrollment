# Enrollment API

A REST API built with .NET Core 8 for managing customer enrollment with Multi-Factor Authentication (MFA) support.

## Features

- **Customer Management**: CRUD operations for customer enrollment
- **MFA Support**: Time-based One-Time Password (TOTP) implementation
- **Document Management**: Track enrollment documents
- **Search & Pagination**: Advanced search capabilities with pagination
- **Swagger Documentation**: Interactive API documentation
- **Entity Framework Core**: SQL Server database with code-first approach

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

## Testing

The API includes Swagger UI for testing endpoints. Navigate to the root URL to access the interactive documentation.

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
