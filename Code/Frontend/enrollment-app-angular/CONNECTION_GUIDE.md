# Angular-Backend Connection Guide

This guide explains how to connect your Angular enrollment app to the .NET backend API.

## Prerequisites

- .NET 8.0 SDK
- Node.js (for Angular)
- Angular CLI

## Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd Code/Backend/EnrollmentApi
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the backend API:
   ```bash
   dotnet run
   ```

The API will start on `http://localhost:5065` (or the next available port).

## Frontend Setup

1. Navigate to the Angular app directory:
   ```bash
   cd Code/Frontend/enrollment-app-angular
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the Angular development server:
   ```bash
   npm start
   ```

The Angular app will start on `http://localhost:4200`.

## API Endpoints

### Customer Locate Endpoint
- **URL**: `GET /api/customers/locate`
- **Parameters**:
  - `accountNumber`: The customer's account number
  - `ssn`: The customer's SSN
  - `birthdate`: The customer's birthdate (YYYY-MM-DD format)

### Example Request
```
GET http://localhost:5065/api/customers/locate?accountNumber=1234567890123456&ssn=123456789&birthdate=1990-01-01
```

## Testing the Connection

1. **Start both applications** (backend and frontend)

2. **Open the Angular app** in your browser at `http://localhost:4200`

3. **Navigate to the Customer Locate page**

4. **Fill in the form** with test data:
   - Account Number: `1234567890123456`
   - SSN: `123456789`
   - Birthdate: `1990-01-01`

                               **Note:** This test data corresponds to "John Doe" in the seeded database. When you submit this data, you should see a success message showing "Welcome back, John Doe" and the form will become read-only before redirecting to the next step. In the account setup step, the full name will be displayed as a read-only label showing "John Doe" and the phone number will be pre-filled, but the email field will remain empty for the user to fill in manually.

5. **Click "Next"** - the app will:
   - Show a loading spinner
   - Make an API call to the backend
   - Either find an existing customer or proceed to create a new one

## API Documentation

Once the backend is running, you can access the Swagger documentation at:
`http://localhost:5065/swagger`

This provides interactive documentation for all available endpoints.

## Test Data

The backend is seeded with test data including:

### Customer: John Doe
- **Account Number**: `1234567890123456`
- **SSN**: `123456789`
- **Birthdate**: `1990-01-01`
- **Email**: `john.doe@example.com`
- **Phone**: `555-123-4567`

You can use this data to test the customer locate functionality. When you enter these credentials, the system will find the existing customer and display a success message.

## Troubleshooting

### CORS Issues
The backend is configured with CORS to allow requests from any origin. If you encounter CORS issues, check that the backend is running and the CORS policy is properly configured in `Program.cs`.

### Port Conflicts
If port 5065 is already in use, the backend will automatically use the next available port. Check the console output for the actual URL.

### Network Issues
Ensure both applications are running and accessible. You can test the API directly using:
- Swagger UI at `http://localhost:5065/swagger`
- curl or Postman

## Data Flow

1. User fills out the customer locate form
2. Angular component validates the form data
3. Customer service makes HTTP request to backend
4. Backend searches for customer using account number, SSN, and birthdate
5. If customer is found, Angular emits the customer data
6. If customer is not found, Angular proceeds to the next step (create new customer)

## Configuration

### Backend URL
The Angular service is configured to connect to `http://localhost:5065`. If your backend runs on a different port, update the `baseUrl` in `src/app/services/customer.service.ts`.

### Environment Variables
For production, consider using environment variables to configure the API URL:
- Create `src/environments/environment.ts` for development
- Create `src/environments/environment.prod.ts` for production
- Update the service to use the environment configuration
