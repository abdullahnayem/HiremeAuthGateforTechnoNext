# 🔐 HiremeAuthGate - Advanced Web Authentication System

A comprehensive ASP.NET Core 8.0 web application demonstrating enterprise-level authentication, security best practices, and modern web development patterns.

## 🎯 Key Technologies & Features

### **Backend Technologies**
• **ASP.NET Core 8.0** - Latest .NET framework with performance optimizations
• **Entity Framework Core 8.0** - Modern ORM with SQL Server integration
• **BCrypt.Net-Next 4.0.3** - Industry-standard password hashing with work factor 12
• **Dapper 2.1.28** - High-performance micro-ORM for data access
• **Serilog 8.0.0** - Structured logging with file and console sinks
• **Microsoft.Extensions.Configuration.Binder** - Configuration management
• **Microsoft.Extensions.Logging.Abstractions** - Logging infrastructure

### **Frontend Technologies**
• **Bootstrap 5** - Modern responsive CSS framework
• **jQuery Validation** - Client-side form validation
• **jQuery Validation Unobtrusive** - Unobtrusive validation integration
• **Custom CSS/JS** - Tailored styling and interactive features

### **Architecture & Design Patterns**
• **Layered Architecture** - Separation of concerns across BusinessModel, Services, and Web layers
• **Repository Pattern** - Data access abstraction with interface-based design
• **Dependency Injection** - Loose coupling and testability
• **MVC Pattern** - Model-View-Controller separation
• **DTO Pattern** - Data transfer objects for API contracts
• **Result Pattern** - Generic result wrapper for error handling

### **Security Features**
• **Account Locking** - Configurable lockout after failed attempts (5 attempts = 15-minute lock)
• **Password Security** - BCrypt hashing with work factor 12
• **Session Management** - Secure cookie-based authentication
• **Input Validation** - Comprehensive client and server-side validation
• **Global Exception Handling** - Centralized error handling and logging
• **HTTPS Enforcement** - Secure cookie policies and HTTPS redirection

### **Database & Data Access**
• **SQL Server** - Enterprise database with LocalDB support
• **Entity Framework Migrations** - Version-controlled schema management
• **Dapper Integration** - High-performance data access for complex queries
• **Connection String Security** - Trusted connections with certificate validation

### **Development & DevOps**
• **Structured Logging** - Serilog with daily rolling file logs
• **Configuration Management** - JSON-based settings with environment support
• **Global Exception Middleware** - Centralized error handling
• **Database Auto-Migration** - Automatic schema updates on startup
• **XML Documentation** - Comprehensive code documentation

### **User Experience**
• **Responsive Design** - Mobile-first Bootstrap 5 implementation
• **Form Validation** - Real-time client-side and server-side validation
• **User Feedback** - Success/error messages with TempData
• **Modern UI** - Clean, professional interface design
• **Accessibility** - Semantic HTML and ARIA support

---

# 📖 Complete Project Documentation

## 🏗️ Architecture Overview

HiremeAuthGate implements a clean, layered architecture that promotes maintainability, testability, and scalability:

```
HiremeAuthGate/
├── BusinessModel/     # Data Transfer Objects & Domain Models
├── Services/         # Business Logic & Data Access Layer
└── Web/             # Presentation Layer (MVC)
```

### **Design Principles**
- **Separation of Concerns**: Each layer has distinct responsibilities
- **Dependency Inversion**: High-level modules don't depend on low-level modules
- **Interface Segregation**: Clients depend only on interfaces they use
- **Single Responsibility**: Each class has one reason to change

## 🎯 Core Features

### **Authentication System**
- ✅ **User Registration**: Email validation with password confirmation
- ✅ **Secure Login**: BCrypt password verification with account locking
- ✅ **Session Management**: Cookie-based authentication with security policies
- ✅ **Account Security**: Configurable lockout mechanism and activity tracking
- ✅ **Password Policies**: Minimum length validation and secure hashing

### **User Interface**
- ✅ **Responsive Design**: Bootstrap 5 with mobile-first approach
- ✅ **Form Validation**: Real-time client-side and comprehensive server-side validation
- ✅ **User Feedback**: Contextual success/error messages
- ✅ **Modern UI**: Professional design with accessibility features

### **Data Management**
- ✅ **Entity Framework Core**: Full-featured ORM with migration support
- ✅ **Dapper Integration**: High-performance data access for complex operations
- ✅ **Database Migrations**: Version-controlled schema evolution
- ✅ **Data Validation**: Model validation with custom business rules

## 🛠️ Technology Stack

### **Backend Framework**
- **ASP.NET Core 8.0**: Latest .NET framework with performance improvements
- **C# 12**: Modern language features and syntax
- **Nullable Reference Types**: Enhanced type safety
- **Implicit Usings**: Simplified import statements

### **Database & ORM**
- **SQL Server**: Enterprise database with LocalDB for development
- **Entity Framework Core 8.0**: Modern ORM with LINQ support
- **Dapper 2.1.28**: Micro-ORM for high-performance data access
- **Database Migrations**: Automated schema management

### **Security & Authentication**
- **BCrypt.Net-Next 4.0.3**: Industry-standard password hashing
- **Cookie Authentication**: Secure session management
- **Account Locking**: Brute-force protection mechanism
- **Input Validation**: Comprehensive validation pipeline

### **Logging & Monitoring**
- **Serilog 8.0.0**: Structured logging framework
- **File Sinks**: Daily rolling log files
- **Console Sinks**: Development-time logging
- **Structured Data**: JSON-formatted log entries

### **Frontend Technologies**
- **Bootstrap 5**: Modern CSS framework
- **jQuery**: DOM manipulation and AJAX
- **jQuery Validation**: Client-side form validation
- **Custom CSS/JS**: Tailored styling and interactions

## 📁 Project Structure

```
HiremeAuthGate/
├── HiremeAuthGate.BusinessModel/
│   ├── BaseViewModel/
│   │   └── User.cs                    # User entity with validation
│   ├── DTOs/
│   │   ├── LoginRequest.cs           # Login data transfer object
│   │   └── RegisterRequest.cs        # Registration data transfer object
│   └── Results/
│       └── Result.cs                 # Generic result wrapper
├── HiremeAuthGate.Services/
│   ├── Data/
│   │   └── ApplicationDbContext.cs   # EF Core database context
│   ├── Interfaces/
│   │   ├── IAuthService.cs           # Authentication service contract
│   │   └── IUserRepository.cs        # User repository contract
│   ├── Repositories/
│   │   └── UserRepository.cs         # User data access implementation
│   ├── Services/
│   │   └── AuthService.cs            # Authentication business logic
│   └── Migrations/                   # Database migration files
└── HiremeAuthGate.Web/
    ├── Controllers/
    │   ├── AccountController.cs      # Authentication controller
    │   └── HomeController.cs         # Home page controller
    ├── Middleware/
    │   └── GlobalExceptionHandlerMiddleware.cs  # Error handling
    ├── Views/
    │   ├── Account/
    │   │   ├── Login.cshtml          # Login page view
    │   │   └── Register.cshtml       # Registration page view
    │   ├── Home/
    │   │   ├── Success.cshtml        # Success page view
    │   │   └── Error.cshtml          # Error page view
    │   └── Shared/
    │       └── _Layout.cshtml        # Master layout page
    ├── wwwroot/
    │   ├── css/
    │   │   ├── auth.css              # Authentication styles
    │   │   ├── site.css              # Main site styles
    │   │   └── success-error.css     # Success/error page styles
    │   ├── js/
    │   │   └── form-validation.js    # Custom validation scripts
    │   └── lib/                      # Third-party libraries
    ├── Program.cs                    # Application entry point
    └── appsettings.json              # Configuration file
```

## 🚀 Getting Started

### **Prerequisites**
- .NET 8.0 SDK or later
- Visual Studio 2022 (17.8+) or VS Code
- SQL Server (LocalDB recommended for development)
- Git for version control

### **Installation & Setup**

1. **Clone the repository**
```bash
git clone <repository-url>
cd HiremeAuthGate
```

2. **Configure database connection**
Update `HiremeAuthGate.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=(localdb)\\mssqllocaldb;Database=HiremeAuthGatedb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

3. **Restore dependencies**
```bash
cd HiremeAuthGate.Web
dotnet restore
```

4. **Run database migrations**
```bash
dotnet ef database update
```

5. **Run the application**
```bash
dotnet run
```

6. **Access the application**
Open browser and navigate to: `https://localhost:5001` or `http://localhost:5000`

## 🎮 Usage Guide

### **Registration Process**
1. Navigate to `/Account/Register`
2. Enter a valid email address (format validation)
3. Enter password (minimum 6 characters)
4. Confirm password (must match)
5. Submit registration
6. Redirected to login page with success message

### **Login Process**
1. Navigate to `/Account/Login`
2. Enter registered email address
3. Enter password
4. Submit login credentials
5. Redirected to success page or error page based on result

### **Security Features**
- **Account Locking**: After 5 failed attempts, account is locked for 15 minutes
- **Password Hashing**: BCrypt with work factor 12 for secure storage
- **Session Security**: HttpOnly cookies with secure settings
- **Input Validation**: Comprehensive client and server-side validation
- **Error Handling**: Centralized exception handling with logging

## 🔧 Configuration

### **Authentication Settings**
```csharp
// Program.cs
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });
```

### **Security Configuration**
```json
{
  "Security": {
    "MaxLoginAttempts": 5,
    "LockoutDurationMinutes": 15
  }
}
```

### **Logging Configuration**
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

## 🧪 Testing Strategy

### **Manual Test Cases**
1. **Registration Flow**
   - Valid email format validation
   - Password minimum length (6 characters)
   - Password confirmation matching
   - Duplicate email handling

2. **Login Flow**
   - Valid credentials authentication
   - Invalid credentials handling
   - Account locking mechanism
   - Session management

3. **Security Testing**
   - Account lockout after 5 failed attempts
   - Lockout duration verification (15 minutes)
   - Password hashing verification
   - Session security validation

4. **UI/UX Testing**
   - Responsive design on different screen sizes
   - Form validation feedback
   - Error message display
   - Success message handling

### **Security Testing Checklist**
- ✅ SQL Injection prevention (parameterized queries)
- ✅ XSS protection (input sanitization)
- ✅ CSRF protection (anti-forgery tokens)
- ✅ Password strength validation
- ✅ Account locking mechanism
- ✅ Secure cookie settings
- ✅ HTTPS enforcement

## 📝 API Documentation

### **Account Controller Endpoints**

#### **GET /Account/Login**
- **Purpose**: Display login form
- **Parameters**: `returnUrl` (optional) - URL to redirect after login
- **Returns**: Login view with form

#### **POST /Account/Login**
- **Purpose**: Authenticate user credentials
- **Parameters**: `LoginRequest` model containing email and password
- **Returns**: Redirect to success page, error page, or return URL
- **Security**: Account locking, password verification, session creation

#### **GET /Account/Register**
- **Purpose**: Display registration form
- **Returns**: Registration view with form

#### **POST /Account/Register**
- **Purpose**: Register new user account
- **Parameters**: `RegisterRequest` model containing email, password, and confirmation
- **Returns**: Redirect to login page with success message
- **Validation**: Email format, password strength, confirmation matching

### **Home Controller Endpoints**

#### **GET /Home/Success**
- **Purpose**: Display success page after login
- **Access**: Requires authentication
- **Returns**: Success view with user information

#### **GET /Home/Error**
- **Purpose**: Display error page
- **Returns**: Error view with error details

## 🔒 Security Implementation

### **Password Security**
- **Hashing Algorithm**: BCrypt with work factor 12
- **Validation Rules**: Minimum 6 characters
- **Confirmation**: Required password confirmation
- **Storage**: Hashed passwords only, never plain text

### **Account Protection**
- **Locking Mechanism**: 5 failed attempts = 15-minute lockout
- **Session Security**: Secure, HttpOnly cookies
- **Input Validation**: Comprehensive validation pipeline
- **Error Handling**: Secure error messages without information disclosure

### **Data Protection**
- **SQL Injection**: Parameterized queries via EF Core and Dapper
- **XSS Prevention**: Input sanitization and output encoding
- **CSRF Protection**: Anti-forgery tokens in forms
- **HTTPS Enforcement**: Secure cookie policies and redirects

## 📊 Database Schema

### **Users Table**
```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    IsActive BIT DEFAULT 1,
    LoginAttempts INT DEFAULT 0,
    LockedUntil DATETIME2 NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);
```

### **Indexes**
```sql
-- Email index for fast lookups
CREATE UNIQUE INDEX IX_Users_Email ON Users(Email);

-- Active users index
CREATE INDEX IX_Users_IsActive ON Users(IsActive);
```

## 🚀 Deployment

### **Production Considerations**
1. **Database**: Use full SQL Server instance (not LocalDB)
2. **Connection String**: Use secure connection strings with proper credentials
3. **HTTPS**: Ensure SSL certificate is properly configured
4. **Logging**: Configure appropriate log levels for production
5. **Security**: Review and update security settings as needed

### **Environment Configuration**
```json
{
  "ConnectionStrings": {
    "Default": "Server=your-server;Database=HiremeAuthGatedb;User Id=your-user;Password=your-password;TrustServerCertificate=true;"
  },
  "Security": {
    "MaxLoginAttempts": 5,
    "LockoutDurationMinutes": 15
  }
}
```

### **Code Quality**
- Use meaningful variable and method names
- Implement proper exception handling
- Follow SOLID principles
- Add comprehensive logging
- Maintain security best practices


**Thanks - TechnoNext**
