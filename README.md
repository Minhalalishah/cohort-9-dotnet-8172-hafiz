
# 🚀 Task Management Tool

A full-stack web-based **Task Management System** built with **ASP.NET Core 8 Web API, React, TypeScript, Entity Framework Core, and Microsoft SQL Server**.

## ✨ Features

* 🔐 User Registration & Login
* 🔑 JWT Authentication & Role-Based Authorization
* 📋 Task CRUD Operations
* 👥 Task Assignment
* 📊 Dashboard & Task Statistics
* 🏷️ Task Categories, Priorities & Status
* 🔎 Search & Filtering
* 👤 User Profile
* 📝 Serilog Logging
* 🛡️ Global Exception Handling
* 🧪 xUnit Unit Testing
* 📈 SonarQube Code Quality Analysis
* 🌿 Git & GitHub Version Control

**Technologies:** React, TypeScript, ASP.NET Core, C#, SQL Server

## 🛠️ Tech Stack

### Frontend

* React 19
* TypeScript
* Vite
* React Router
* Axios
* CSS3

### Backend

* ASP.NET Core 8 Web API
* C#
* Entity Framework Core 8
* REST API
* JWT Authentication
* BCrypt

### Database

* Microsoft SQL Server
* SQL Server Express
* Entity Framework Core
* LINQ

### Tools

* Serilog
* xUnit
* SonarQube
* Git
* GitHub

## 🏗️ Architecture

```text
React + TypeScript
        │
        ▼
ASP.NET Core 8 Web API
        │
        ▼
Entity Framework Core
        │
        ▼
Microsoft SQL Server
```

## 📁 Project Structure

```text
TaskManagementTool/
├── backend/
│   ├── TaskManagement.Api/
│   └── TaskManagement.Tests/
├── frontend/
│   └── src/
├── docs/
├── scripts/
├── .gitignore
└── README.md
```

## 🚀 Quick Start

### Backend

```powershell
cd backend
dotnet restore
dotnet build
dotnet ef database update --project TaskManagement.Api
dotnet run --project TaskManagement.Api
```

**Backend:** `http://localhost:5000`

**Swagger:** `http://localhost:5000/swagger`

### Frontend

```powershell
cd frontend
npm install
npm run dev
```

**Frontend:** `http://localhost:5173`

## 🌿 Git Workflow

```text
main
 │
 └── develop
      ├── feature/authentication
      ├── feature/task-management
      ├── feature/dashboard
      ├── feature/profile
      ├── feature/logging
      ├── feature/testing
      └── feature/sonarqube
```

Feature branches are created from `develop` and merged back after completion.

## 🧪 Testing

Run backend tests:

```powershell
cd backend
dotnet test
```

## 📊 Project Status

| Component             | Status |
| --------------------- | ------ |
| React + TypeScript    | ✅      |
| ASP.NET Core 8        | ✅      |
| SQL Server            | ✅      |
| Entity Framework Core | ✅      |
| JWT Authentication    | ✅      |
| Task CRUD             | ✅      |
| Dashboard             | ✅      |
| Serilog Logging       | ✅      |
| Exception Handling    | ✅      |
| xUnit Testing         | ✅      |
| SonarQube             | ✅      |
| Git/GitHub            | ✅      |

## 👨‍💻 Author

**Hafiz Syed Minhal Ali**
**cohort-9-dotnet-8172-hafiz**







