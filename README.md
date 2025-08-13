# POC Enrollment Project

A comprehensive enrollment system built as a monorepo with modern frontend and backend components for account opening and management.

## 🏗️ Project Structure

```
POC_ENROLLMENT/
├── Code/
│   ├── Frontend/                    # React-based enrollment applications
│   │   ├── enrollment-app/          # Account opening React app
│   │   └── enrollment-app-angular/  # Angular-based enrollment app
│   └── Backend/                     # .NET Core API with comprehensive testing
│       ├── EnrollmentApi/           # Main API project
│       └── EnrollmentApi.Tests/     # Test suite (Unit + Integration)
├── .gitignore                       # Git ignore rules for entire project
└── README.md                        # This file
```

## 🚀 Features

### Frontend (React App)
- **Multi-step form process** with progress indicator
- **Customer Locate** step for account identification
- **Setup Account** step for user credentials
- **Thank You** confirmation page
- **Form validation** with error handling
- **Responsive design** with Tailwind CSS
- **Modern UI** with smooth transitions and animations

### Backend ✅ (Complete)
- **REST API** built with .NET Core 8
- **Customer Management** with CRUD operations
- **Multi-Factor Authentication (MFA)** using TOTP
- **Entity Framework Core** with in-memory database
- **Comprehensive Validation** using FluentValidation
- **Swagger Documentation** for API testing
- **Comprehensive Test Suite** with 71 tests (100% pass rate)

## 🛠️ Tech Stack

### Frontend
- **React 18** - Modern UI framework
- **Vite** - Fast build tool and dev server
- **Tailwind CSS** - Utility-first CSS framework
- **Vanilla JavaScript** - No TypeScript for simplicity

### Backend ✅ (Complete)
- **.NET Core 8** - Modern, high-performance framework
- **Entity Framework Core** - ORM with in-memory database
- **FluentValidation** - Comprehensive input validation
- **Swagger/OpenAPI** - Interactive API documentation
- **xUnit + Moq + Shouldly** - Comprehensive testing framework
- **Multi-Factor Authentication** - TOTP implementation

## 📦 Getting Started

### Prerequisites

- **Node.js** (version 16 or higher) - For frontend development
- **.NET 8.0 SDK** - For backend development
- **Git** for version control
- **Code editor** (VS Code recommended)

### Frontend Setup

1. **Navigate to the Frontend directory:**
   ```bash
   cd Code/Frontend/enrollment-app
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Start the development server:**
   ```bash
   npm run dev
   ```

4. **Open your browser and navigate to `http://localhost:3000`**

### Backend Setup ✅

1. **Navigate to the Backend directory:**
   ```bash
   cd Code/Backend/EnrollmentApi
   ```

2. **Install .NET 8.0 SDK** (if not already installed)

3. **Run the API:**
   ```bash
   dotnet run
   ```

4. **Access the API:**
   - **API**: http://localhost:5065
   - **Swagger UI**: http://localhost:5065/swagger

5. **Run Tests:**
   ```bash
   cd ../EnrollmentApi.Tests
   dotnet test
   ```

## 📁 Project Structure

### Frontend Structure
```
Code/Frontend/enrollment-app/
├── src/
│   ├── components/
│   │   ├── CustomerLocate.jsx    # Step 1: Customer identification
│   │   ├── SetupAccount.jsx      # Step 2: Account setup
│   │   └── ThankYou.jsx          # Step 3: Confirmation
│   ├── App.jsx                   # Main app component
│   ├── main.jsx                  # Entry point
│   └── index.css                 # Global styles with Tailwind
├── public/
├── package.json
├── vite.config.js
├── tailwind.config.js
└── README.md
```

### Backend Structure ✅
```
Code/Backend/
├── EnrollmentApi/                    # Main API project
│   ├── Controllers/                  # API controllers
│   ├── Models/                      # Data models
│   ├── DTOs/                        # Data transfer objects
│   ├── Services/                    # Business logic services
│   ├── Validators/                  # FluentValidation rules
│   ├── Data/                        # Entity Framework context
│   └── Program.cs                   # Application entry point
├── EnrollmentApi.Tests/             # Comprehensive test suite
│   ├── Unit/                        # Unit tests
│   │   ├── CustomerServiceTests.cs  # Service layer tests
│   │   └── ValidatorTests.cs        # Validation tests
│   ├── Integration/                 # Integration tests
│   │   └── CustomerLocateIntegrationTests.cs
│   └── README.md                    # Test documentation
└── EnrollmentSolution.sln           # Solution file
```

## 🔄 Development Workflow

### Available Scripts (Frontend)

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build

### Form Flow

1. **Customer Locate**: Enter account number, SSN, and birthdate
2. **Setup Account**: Create email, username, and password
3. **Thank You**: Confirmation page with success message

## 🎨 Frontend Features

- **Progress Indicator**: Visual progress bar showing current step
- **Form Validation**: Real-time validation with error messages
- **Responsive Design**: Works on desktop and mobile devices
- **Smooth Transitions**: CSS animations for better UX
- **Error Handling**: Comprehensive form validation
- **Back Navigation**: Ability to go back to previous steps

## 🔧 Configuration

### Frontend Customization

The app uses Tailwind CSS for styling. You can customize the design by:

1. Modifying the `tailwind.config.js` file for theme changes
2. Updating the `src/index.css` file for custom styles
3. Modifying component styles in individual `.jsx` files

### Environment Variables

Create `.env` files in respective directories for environment-specific configurations:

- `Code/Frontend/enrollment-app/.env` - Frontend environment variables
- `Code/Backend/.env` - Backend environment variables (future)

## 🧪 Testing

### Backend Testing ✅ (Complete)
The backend includes comprehensive test coverage with both unit tests and integration tests.

#### Test Structure
```
Code/Backend/EnrollmentApi.Tests/
├── Unit/                           # Unit tests
│   ├── CustomerServiceTests.cs     # CustomerService unit tests
│   └── ValidatorTests.cs           # FluentValidation tests
├── Integration/                    # Integration tests
│   └── CustomerLocateIntegrationTests.cs  # API endpoint tests
└── README.md                       # Detailed test documentation
```

#### Test Coverage
- **Total Tests**: 71
- **Unit Tests**: 63 (CustomerService + Validators)
- **Integration Tests**: 8 (API endpoints)
- **Pass Rate**: 100% ✅

#### Running Tests
```bash
cd Code/Backend/EnrollmentApi.Tests
dotnet test
```

#### Test Categories
- **Unit Tests**: Test individual components in isolation using in-memory database
- **Integration Tests**: Test API endpoints end-to-end with HTTP requests
- **Validation Tests**: Test all DTO validation rules using FluentValidation

#### Test Dependencies
- **xUnit** (2.9.2) - Testing framework
- **Moq** (4.20.70) - Mocking framework
- **Shouldly** (4.2.1) - Readable assertions
- **FluentAssertions** (8.5.0) - HTTP response assertions
- **Microsoft.EntityFrameworkCore.InMemory** (9.0.8) - In-memory database
- **Microsoft.AspNetCore.Mvc.Testing** (9.0.8) - Integration testing

#### Test Documentation
For detailed information about the test suite, including:
- Test organization and structure
- Running specific test categories
- Writing new tests
- Debugging test issues

See: [Test Documentation](Code/Backend/EnrollmentApi.Tests/README.md)

### Frontend Testing (Future)
- Unit tests with Jest and React Testing Library
- Integration tests for form flows
- E2E tests with Cypress or Playwright

## 📚 API Documentation ✅

The backend API includes comprehensive documentation and testing capabilities:

### Interactive API Documentation
- **Swagger UI**: http://localhost:5065/swagger
- **OpenAPI Spec**: http://localhost:5065/swagger/v1/swagger.json

### API Endpoints

#### Customer Management
- `GET /api/customers` - Get all customers with search/pagination
- `GET /api/customers/{id}` - Get customer by ID
- `GET /api/customers/email/{email}` - Get customer by email
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer

#### MFA Operations
- `POST /api/mfa/setup/{customerId}` - Setup MFA for customer
- `POST /api/mfa/verify` - Verify MFA code
- `POST /api/mfa/enable` - Enable MFA
- `POST /api/mfa/disable` - Disable MFA
- `GET /api/mfa/status/{customerId}` - Get MFA status

### API Testing
- Use Swagger UI for interactive testing
- Run integration tests: `dotnet test --filter "Integration"`
- See [API Documentation](Code/Backend/EnrollmentApi/README.md) for detailed examples

## 🚀 Deployment

### Frontend Deployment
- **Development**: `npm run dev`
- **Production**: `npm run build` then serve `dist/` folder
- **Platforms**: Vercel, Netlify, AWS S3, or any static hosting

### Backend Deployment (Future)
- **Development**: Local server with hot reload
- **Production**: Docker containers, cloud platforms (AWS, Azure, GCP)
- **Database**: Managed database services

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Create an issue in the GitHub repository
- Contact the development team
- Check the documentation in each component's README

## 🔮 Roadmap

### Phase 1 ✅ (Complete)
- [x] Frontend React application
- [x] Multi-step form implementation
- [x] Responsive design with Tailwind CSS
- [x] Form validation and error handling

### Phase 2 ✅ (Complete)
- [x] Backend API development (.NET Core 8)
- [x] Database integration (Entity Framework Core)
- [x] Authentication system (MFA with TOTP)
- [x] API documentation (Swagger/OpenAPI)
- [x] Comprehensive test suite (71 tests, 100% pass rate)
- [x] Input validation (FluentValidation)

### Phase 3 📋 (Planned)
- [ ] User management system
- [ ] Advanced form features
- [ ] Analytics and reporting
- [ ] Mobile app development
- [ ] Integration with external services

---

**Built with ❤️ using React, Tailwind CSS, and modern web technologies** 