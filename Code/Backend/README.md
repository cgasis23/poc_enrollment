# Enrollment Backend

This folder contains the backend API for the enrollment system built with .NET Core 8.

## Solution Structure

```
Backend/
├── EnrollmentSolution.sln          # Visual Studio Solution file
├── EnrollmentApi/                  # Main API project
│   ├── Controllers/                # API Controllers
│   ├── Data/                       # Entity Framework DbContext
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Models/                     # Entity Models
│   ├── Services/                   # Business Logic Services
│   └── README.md                   # API Documentation
└── README.md                       # This file
```

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Running the Solution

1. **Open the Solution:**
   ```bash
   # Open in Visual Studio
   EnrollmentSolution.sln
   
   # Or use VS Code
   code .
   ```

2. **Build the Solution:**
   ```bash
   dotnet build EnrollmentSolution.sln
   ```

3. **Run the API:**
   ```bash
   cd EnrollmentApi
   dotnet run
   ```

4. **Access the API:**
   - **API Base URL**: http://localhost:5065
   - **Swagger UI**: http://localhost:5065/swagger

## Features

- **Customer Management**: Full CRUD operations for customer enrollment
- **MFA Support**: Time-based One-Time Password (TOTP) implementation
- **Document Management**: Track enrollment documents and notes
- **Search & Pagination**: Advanced search capabilities
- **FluentValidation**: Comprehensive input validation with custom rules
- **In-Memory Database**: Perfect for development and testing
- **Swagger Documentation**: Interactive API documentation

## API Endpoints

### Customers
- `GET /api/customers` - Get all customers with search/pagination
- `GET /api/customers/{id}` - Get customer by ID
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer

### MFA
- `POST /api/mfa/setup/{customerId}` - Setup MFA for customer
- `POST /api/mfa/verify` - Verify MFA code
- `POST /api/mfa/enable` - Enable MFA
- `GET /api/mfa/status/{customerId}` - Get MFA status

## Development

### Adding New Projects
1. Create new project in the Backend folder
2. Add project to solution: `dotnet sln add ProjectName/ProjectName.csproj`
3. Update this README with new project information

### Database
- **Development**: In-memory database (no setup required)
- **Production**: SQL Server (configure in appsettings.Production.json)

## Testing

Use the Swagger UI at http://localhost:5065/swagger to:
- View all available endpoints
- Test API calls directly
- See request/response schemas
- Try out different parameters

## Deployment

1. Switch to SQL Server for production
2. Configure connection strings
3. Set up proper CORS policy
4. Enable authentication/authorization
5. Configure logging and monitoring

For detailed API documentation, see [EnrollmentApi/README.md](EnrollmentApi/README.md).
