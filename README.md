# 🔐 HiremeAuthGate - Web Authentication System

A complete ASP.NET Core web application demonstrating modern web development practices, secure authentication, and database integration.

## 📖 Overview

HiremeAuthGate is a full-featured authentication system built with ASP.NET Core 8.0. It showcases professional web development practices including layered architecture, secure authentication, database integration, and responsive UI design.

## 🏗️ Architecture

### Layered Architecture
```
HiremeAuthGate/
├── BusinessModel/     # Data Transfer Objects & Models
├── Services/         # Business Logic & Data Access
└── Web/             # Presentation Layer (MVC)
```

### Design Patterns
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Loose coupling
- **MVC Pattern**: Model-View-Controller separation
- **DTO Pattern**: Data transfer objects

## 🎯 Core Features

### Authentication & Security
- ✅ **User Registration**: Email validation and password confirmation
- ✅ **Secure Login**: BCrypt password hashing
- ✅ **Account Locking**: Security mechanism for failed attempts
- ✅ **Session Management**: Cookie-based authentication
- ✅ **Password Security**: Minimum length and validation

### User Interface
- ✅ **Responsive Design**: Bootstrap 5 framework
- ✅ **Form Validation**: Client and server-side validation
- ✅ **User Feedback**: Success/error messages
- ✅ **Modern UI**: Clean and professional design

### Database Integration
- ✅ **Entity Framework Core**: ORM with SQL Server
- ✅ **Database Migrations**: Version-controlled schema changes
- ✅ **Data Validation**: Model validation and constraints

## 🛠️ Technologies

### Backend
- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: Cookie-based with BCrypt hashing
- **Validation**: Data Annotations & Model Validation

### Frontend
- **UI Framework**: Bootstrap 5
- **JavaScript**: jQuery Validation
- **CSS**: Custom styling with Bootstrap
- **Responsive**: Mobile-first design

### Development
- **Language**: C# (.NET 8.0)
- **Architecture**: Layered Architecture
- **Documentation**: XML Comments
- **Security**: Account locking, password hashing

## 📁 Project Structure

```
HiremeAuthGate/
├── HiremeAuthGate.BusinessModel/
│   ├── BaseViewModel/
│   │   └── User.cs                    # User entity model
│   ├── DTOs/
│   │   ├── LoginRequest.cs           # Login data transfer object
│   │   └── RegisterRequest.cs        # Registration data transfer object
│   └── Results/
│       └── Result.cs                 # Generic result wrapper
├── HiremeAuthGate.Services/
│   ├── Data/
│   │   └── ApplicationDbContext.cs   # Entity Framework context
│   ├── Interfaces/
│   │   ├── IAuthService.cs           # Authentication service interface
│   │   └── IUserRepository.cs        # User repository interface
│   ├── Repositories/
│   │   └── UserRepository.cs         # User data access implementation
│   ├── Services/
│   │   └── AuthService.cs            # Authentication business logic
│   └── Migrations/                   # Database migration files
└── HiremeAuthGate.Web/
    ├── Controllers/
    │   ├── AccountController.cs      # Authentication controller
    │   └── HomeController.cs         # Home page controller
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
    │   │   └── site.css              # Custom styles
    │   ├── js/
    │   │   └── form-validation.js    # Custom JavaScript
    │   └── lib/                      # Third-party libraries
    ├── Program.cs                    # Application entry point
    └── appsettings.json              # Configuration file
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server (LocalDB or full instance)

### Installation & Setup

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

## 🎮 Usage

### Registration Process
1. Navigate to `/Account/Register`
2. Enter valid email address
3. Enter password (minimum 6 characters)
4. Confirm password
5. Submit registration
6. Redirected to login page

### Login Process
1. Navigate to `/Account/Login`
2. Enter registered email
3. Enter password
4. Submit login
5. Redirected to success page or error page

### Security Features
- **Account Locking**: After 5 failed attempts, account is locked for 15 minutes
- **Password Hashing**: BCrypt with work factor 12
- **Session Security**: HttpOnly cookies with secure settings
- **Input Validation**: Client and server-side validation

## 🔧 Configuration

### Authentication Settings
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
    });
```

### Database Configuration
```json
{
  "ConnectionStrings": {
    "Default": "Server=your-server;Database=HiremeAuthGatedb;Trusted_Connection=true;"
  }
}
```

## 🧪 Testing

### Manual Test Cases
1. **Registration**: Test with valid/invalid email formats
2. **Password Validation**: Test minimum length and confirmation
3. **Login**: Test with correct/incorrect credentials
4. **Account Locking**: Test failed login attempts
5. **Session Management**: Test login/logout flow
6. **Responsive Design**: Test on different screen sizes

### Security Testing
- SQL Injection prevention
- XSS protection
- CSRF protection
- Password strength validation
- Account locking mechanism

## 📝 API Documentation

### Account Controller Endpoints

#### GET /Account/Login
- **Purpose**: Display login form
- **Parameters**: `returnUrl` (optional)
- **Returns**: Login view

#### POST /Account/Login
- **Purpose**: Authenticate user
- **Parameters**: `LoginRequest` model
- **Returns**: Redirect to success/error page

#### GET /Account/Register
- **Purpose**: Display registration form
- **Returns**: Registration view

#### POST /Account/Register
- **Purpose**: Register new user
- **Parameters**: `RegisterRequest` model
- **Returns**: Redirect to login page

## 🔒 Security Features

### Password Security
- **Hashing**: BCrypt with work factor 12
- **Validation**: Minimum 6 characters
- **Confirmation**: Password confirmation required

### Account Protection
- **Locking**: 3 failed attempts = 15-minute lock
- **Session**: Secure cookie settings
- **Validation**: Comprehensive input validation

### Data Protection
- **SQL Injection**: Parameterized queries
- **XSS**: Input sanitization
- **CSRF**: Anti-forgery tokens

## 📊 Database Schema

### Users Table
```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    IsActive BIT DEFAULT 1,
    LoginAttempts INT DEFAULT 0,
    LockoutEnd DATETIME2 NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);

Thanks- TechnoNext
