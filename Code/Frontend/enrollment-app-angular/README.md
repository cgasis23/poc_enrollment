# Enrollment App - Angular v20

This is an Angular v20 application that replicates the functionality of the React enrollment app. It provides a multi-step enrollment process with two-factor authentication setup.

## Features

- **Multi-step enrollment process**: Customer locate → Account setup → Thank you
- **Two-factor authentication setup**: Email and SMS verification options
- **Form validation**: Real-time validation with error messages
- **Responsive design**: Built with Tailwind CSS
- **Modern Angular features**: Uses Angular v20 with signals and standalone components

## Components

- `AppComponent`: Main application component with step management
- `CustomerLocateComponent`: First step for account number, SSN, and birthdate
- `SetupAccountComponent`: Second step for email, username, password, and phone
- `ThankYouComponent`: Final step showing completion message
- `MFADemoComponent`: Demo component for MFA setup
- `MFASetupComponent`: MFA setup with email/SMS verification
- `MFACompleteComponent`: MFA completion screen

## Services

- `MFAService`: Handles MFA code generation, sending, and verification (mock implementation)

## Getting Started

### Prerequisites

- Node.js (v18 or higher)
- npm or yarn

### Installation

1. Clone the repository
2. Navigate to the project directory:
   ```bash
   cd enrollment-app-angular
   ```

3. Install dependencies:
   ```bash
   npm install
   ```

4. Start the development server:
   ```bash
   npm start
   ```

5. Open your browser and navigate to `http://localhost:4200`

### Build for Production

```bash
npm run build
```

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── customer-locate/
│   │   ├── setup-account/
│   │   ├── thank-you/
│   │   ├── mfa-demo/
│   │   ├── mfa-setup/
│   │   └── mfa-complete/
│   ├── services/
│   │   └── mfa.service.ts
│   ├── app.component.ts
│   ├── app.component.html
│   └── app.component.css
├── styles.css
└── main.ts
```

## Technologies Used

- **Angular v20**: Latest version with signals and standalone components
- **TypeScript**: Type-safe JavaScript
- **Tailwind CSS**: Utility-first CSS framework
- **Angular Forms**: Reactive forms for form handling

## Key Features

### Enrollment Flow
1. **Customer Locate**: Enter account number, SSN, and birthdate
2. **Account Setup**: Create username, password, and provide contact information
3. **Thank You**: Confirmation screen with next steps

### MFA Demo
- **Setup**: Choose between email or SMS verification
- **Verification**: Enter 6-digit code with countdown timer
- **Completion**: Success screen with security status

### Form Validation
- Real-time validation with error messages
- Password confirmation matching
- Email format validation
- Phone number format validation

## Development Notes

- Uses Angular signals for state management
- Standalone components for better tree-shaking
- Mock MFA service for demonstration purposes
- Responsive design with Tailwind CSS
- TypeScript interfaces for type safety

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)
