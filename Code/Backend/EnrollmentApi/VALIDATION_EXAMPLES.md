# FluentValidation Examples

This document provides examples of how to test the validation rules implemented with FluentValidation.

## Testing Validation Rules

### Customer Creation Validation

#### ✅ Valid Customer Creation
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "+1234567890",
  "dateOfBirth": "1990-01-01T00:00:00Z",
  "address": "123 Main St",
  "city": "New York",
  "state": "NY",
  "zipCode": "10001",
  "country": "US",
  "ssn": "123-45-6789"
}
```

#### ❌ Invalid Customer Creation Examples

**Missing Required Fields:**
```json
{
  "firstName": "",
  "lastName": "Doe",
  "email": "invalid-email",
  "phoneNumber": "123"
}
```
*Expected Errors:*
- First name is required
- Please provide a valid email address
- Please provide a valid phone number

**Invalid Name Format:**
```json
{
  "firstName": "John123",
  "lastName": "Doe@",
  "email": "john.doe@example.com",
  "phoneNumber": "+1234567890",
  "dateOfBirth": "1990-01-01T00:00:00Z"
}
```
*Expected Errors:*
- First name can only contain letters, spaces, hyphens, and apostrophes
- Last name can only contain letters, spaces, hyphens, and apostrophes

**Invalid Age:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "+1234567890",
  "dateOfBirth": "2020-01-01T00:00:00Z"
}
```
*Expected Error:*
- Customer must be at least 13 years old

**Invalid SSN Format:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "+1234567890",
  "dateOfBirth": "1990-01-01T00:00:00Z",
  "ssn": "123456789"
}
```
*Expected Error:*
- SSN must be in format XXX-XX-XXXX or XXXXXXXXX

**Invalid Zip Code:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "+1234567890",
  "dateOfBirth": "1990-01-01T00:00:00Z",
  "zipCode": "123"
}
```
*Expected Error:*
- Zip code must be in format XXXXX or XXXXX-XXXX

### Customer Update Validation

#### ✅ Valid Customer Update
```json
{
  "firstName": "Jane",
  "phoneNumber": "+1987654321",
  "status": "Completed"
}
```

#### ❌ Invalid Customer Update
```json
{
  "firstName": "Jane123",
  "phoneNumber": "invalid",
  "dateOfBirth": "2020-01-01T00:00:00Z",
  "status": "InvalidStatus"
}
```
*Expected Errors:*
- First name can only contain letters, spaces, hyphens, and apostrophes
- Please provide a valid phone number
- Customer must be at least 13 years old
- Please provide a valid enrollment status

### MFA Validation

#### ✅ Valid MFA Verification
```json
{
  "customerId": 1,
  "code": "123456"
}
```

#### ❌ Invalid MFA Verification
```json
{
  "customerId": 0,
  "code": "12345"
}
```
*Expected Errors:*
- Customer ID must be greater than 0
- MFA code must be exactly 6 digits

## Testing via Swagger UI

1. Navigate to `http://localhost:5065/swagger`
2. Try the POST `/api/customers` endpoint
3. Use the examples above to test validation
4. Check the response for validation error messages

## Testing via cURL

```bash
# Test invalid customer creation
curl -X POST "http://localhost:5065/api/customers" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "",
    "lastName": "Doe",
    "email": "invalid-email",
    "phoneNumber": "123",
    "dateOfBirth": "2020-01-01T00:00:00Z"
  }'
```

## Validation Error Response Format

When validation fails, the API returns a 400 Bad Request with detailed error messages:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "FirstName": [
      "First name is required"
    ],
    "Email": [
      "Please provide a valid email address"
    ],
    "PhoneNumber": [
      "Please provide a valid phone number"
    ],
    "DateOfBirth": [
      "Customer must be at least 13 years old"
    ]
  }
}
```

## Adding New Validation Rules

To add new validation rules:

1. Create a new validator in the `Validators/` folder
2. Inherit from `AbstractValidator<T>`
3. Define rules in the constructor
4. Register the validator in `Program.cs` (automatic if using assembly scanning)

Example:
```csharp
public class MyDtoValidator : AbstractValidator<MyDto>
{
    public MyDtoValidator()
    {
        RuleFor(x => x.Property)
            .NotEmpty().WithMessage("Property is required")
            .MaximumLength(100).WithMessage("Property cannot exceed 100 characters");
    }
}
```
