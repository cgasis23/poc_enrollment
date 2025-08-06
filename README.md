# POC Enrollment Project

A comprehensive enrollment system built as a monorepo with modern frontend and backend components for account opening and management.

## 🏗️ Project Structure

```
POC_ENROLLMENT/
├── Code/
│   ├── Frontend/           # React-based enrollment application
│   │   └── enrollment-app/ # Account opening React app
│   └── Backend/            # Backend services (future development)
├── .gitignore              # Git ignore rules for entire project
└── README.md               # This file
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

### Backend (Future Development)
- API endpoints for account management
- Database integration
- Authentication and authorization
- Business logic implementation

## 🛠️ Tech Stack

### Frontend
- **React 18** - Modern UI framework
- **Vite** - Fast build tool and dev server
- **Tailwind CSS** - Utility-first CSS framework
- **Vanilla JavaScript** - No TypeScript for simplicity

### Backend (Planned)
- **Node.js/Express** or **Python/FastAPI** or **Java/Spring Boot**
- **Database** - PostgreSQL, MySQL, or MongoDB
- **Authentication** - JWT tokens
- **API Documentation** - Swagger/OpenAPI

## 📦 Getting Started

### Prerequisites

- **Node.js** (version 16 or higher)
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

### Backend Setup (Future)

1. **Navigate to the Backend directory:**
   ```bash
   cd Code/Backend
   ```

2. **Follow backend-specific setup instructions** (to be added)

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

### Backend Structure (Future)
```
Code/Backend/
├── src/
│   ├── controllers/     # API controllers
│   ├── models/         # Data models
│   ├── routes/         # API routes
│   ├── middleware/     # Custom middleware
│   └── utils/          # Utility functions
├── tests/              # Test files
├── docs/               # API documentation
└── config/             # Configuration files
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

### Frontend Testing (Future)
- Unit tests with Jest and React Testing Library
- Integration tests for form flows
- E2E tests with Cypress or Playwright

### Backend Testing (Future)
- Unit tests for API endpoints
- Integration tests for database operations
- API testing with tools like Postman or Insomnia

## 📚 API Documentation (Future)

When backend is implemented, API documentation will be available at:
- Swagger UI: `http://localhost:8000/api-docs`
- OpenAPI Spec: `http://localhost:8000/api-docs.json`

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

### Phase 2 🚧 (In Progress)
- [ ] Backend API development
- [ ] Database integration
- [ ] Authentication system
- [ ] API documentation

### Phase 3 📋 (Planned)
- [ ] User management system
- [ ] Advanced form features
- [ ] Analytics and reporting
- [ ] Mobile app development
- [ ] Integration with external services

---

**Built with ❤️ using React, Tailwind CSS, and modern web technologies** 