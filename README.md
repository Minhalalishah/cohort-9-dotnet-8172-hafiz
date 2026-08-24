# 📝 Task Management Tool

The application provides user authentication, role-based authorization, task management, dashboard statistics, SQL Server database integration, application logging, exception handling, unit testing, and code-quality analysis.
---

# 🚀 Project Overview

The Task Management Tool is a web-based application that enables users to efficiently create, organize, assign, update, and track tasks.

The system consists of a **React.js frontend** and an **ASP.NET Core Web API backend** connected to a **SQL Server database** using Entity Framework Core.

The application also includes JWT authentication, role-based authorization, Serilog logging, global exception handling, xUnit testing, SonarQube integration, and Git-based version control.

---

# ✨ Features

## 🔐 Authentication & Authorization

- User Registration
- User Login
- JWT Authentication
- Password Hashing
- Role-Based Authorization
- Admin / Super User access
- Regular User access
- Secure protected API endpoints
- Logout functionality

## 📋 Task Management

- Create Tasks
- View Tasks
- View Task Details
- Update Tasks
- Delete Tasks
- Assign Tasks to Users
- Task Priority
- Task Categories
- Task Due Dates
- Task Status
- Task Filtering
- User-specific Tasks

## 📊 Dashboard

The Dashboard provides:

- Completed Task Count
- In-Progress Task Count
- Pending Task Count

Regular users can view their own task statistics, while authorized administrators can view overall task statistics.

## 👤 User Profile

- View User Information
- Display User Details
- Logout
- Authentication-aware profile

## 🗄️ Database

- SQL Server
- Entity Framework Core
- Database Migrations
- Database Seeding
- User Data
- Task Data
- User/Task Relationships

## 📝 Logging

The backend uses **Serilog** for:

- Application Logging
- API Request Logging
- User Activity Logging
- Error Logging
- Exception Logging

## 🛡️ Exception Handling

Global exception handling is implemented to:

- Catch unexpected errors
- Return consistent API responses
- Provide meaningful error messages
- Log exceptions through Serilog

## 🧪 Testing

- xUnit Unit Testing
- Controller Testing
- Service Testing
- Critical business logic testing
- Backend build verification

## 🔎 Code Quality

- SonarQube
- C# Code Analysis
- JavaScript/TypeScript Code Analysis
- Code Quality Rules
- Potential Bug Detection
- Maintainability Analysis

## 🌿 Version Control

Git and GitHub are used for:

- Feature Branching
- Development Branch
- Pull Requests
- Code Reviews
- Merging
- Version Control

---

# 🛠️ Technology Stack

## Backend

- ASP.NET Core
- C#
- Entity Framework Core
- SQL Server
- JWT
- Serilog

## Frontend

- React.js
- TypeScript / JavaScript
- HTML5
- CSS

## Testing

- xUnit

## Code Quality

- SonarQube

## API Documentation

- Swagger / OpenAPI

## Version Control

- Git
- GitHub


Default development connection is in `backend/TaskManagement.Api/appsettings.Development.json`.

## 2. Backend
```bash
cd backend
dotnet restore
dotnet tool restore
dotnet ef database update --project TaskManagement.Api
dotnet run --project TaskManagement.Api
```
API: `http://localhost:5000`
Swagger: `http://localhost:5000/swagger`

If you do not have `dotnet-ef` globally, install it:
```bash
dotnet tool install --global dotnet-ef
```

## 3. Frontend
```bash
cd frontend
npm install
npm run dev
```
Frontend: `http://localhost:5173`

The frontend API URL is controlled by `VITE_API_URL`.

## Demo accounts
The API seeds these accounts:
- Admin: `admin@tasktool.local` / `Admin@123`
- User: `user@tasktool.local` / `User@123`

Change these credentials before production use.

## API endpoints
- POST `/api/auth/register`
- POST `/api/auth/login`
- GET `/api/users/me`
- GET `/api/dashboard/counts`
- GET `/api/tasks`
- GET `/api/tasks/{id}`
- POST `/api/tasks`
- PUT `/api/tasks/{id}`
- DELETE `/api/tasks/{id}`

## Database migrations
```bash
cd backend
dotnet ef migrations add InitialCreate --project TaskManagement.Api
dotnet ef database update --project TaskManagement.Api
```

## Tests
```bash
cd backend
dotnet test
```

---

# 📂 Project Structure

```text
TaskManagementTool/
│
├── backend/
│   │
│   ├── TaskManagement.Api/
│   │   │
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── TasksController.cs
│   │   │   └── DashboardController.cs
│   │   │
│   │   ├── Data/
│   │   │   ├── AppDbContext.cs
│   │   │   └── DbSeeder.cs
│   │   │
│   │   ├── Middleware/
│   │   │   └── ExceptionHandlingMiddleware.cs
│   │   │
│   │   ├── Models/
│   │   │   ├── Entities.cs
│   │   │   ├── Dtos.cs
│   │   │   └── Task-related Models
│   │   │
│   │   ├── Services/
│   │   │   ├── AuthService.cs
│   │   │   ├── JwtTokenService.cs
│   │   │   ├── TaskService.cs
│   │   │   └── DashboardService.cs
│   │   │
│   │   ├── Migrations/
│   │   │
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── TaskManagement.Api.csproj
│   │
│   └── TaskManagement.sln
│
├── frontend/
│   │
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── services/
│   │   ├── models/
│   │   └── ...
│   │
│   ├── package.json
│   └── ...
│
├── tests/
│   └── Unit Tests
│
├── .gitignore
└── README.md

# 👨‍💻 Author

**Hafiz Syed Minhal Ali**

**Cohort 9-dotnet-8172-hafiz**
