# 📝 Task Management Tool

A full-stack task management application built with **ASP.NET Core 8, React, TypeScript, and SQL Server**.

## ✨ Features

* 🔐 User Authentication & Authorization
* 📝 Task CRUD Operations
* 📊 Dashboard
* 🔑 JWT Authentication
* 🗄️ SQL Server & Entity Framework Core
* 🛡️ Exception Handling
* 📝 Serilog Logging
* 🧪 xUnit Unit Testing

## 🛠️ Technologies

**Backend:** ASP.NET Core 8, C#, Entity Framework Core, SQL Server, JWT, Serilog
**Frontend:** React, TypeScript, Vite
**Testing:** xUnit
**Tools:** Git, GitHub, SonarQube

## 📁 Project Structure

```text
TaskManagementTool/
│
├── backend/
│   │
│   ├── TaskManagement.Api/
│   │   ├── Controllers/
│   │   ├── Data/
│   │   ├── Middleware/
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── Properties/
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── Program.cs
│   │   └── TaskManagement.Api.csproj
│   │
│   └── TaskManagement.Tests/
│       ├── Controllers/
│       ├── Services/
│       └── TaskManagement.Tests.csproj
│
├── frontend/
│   │
│   ├── public/
│   │
│   ├── src/
│   │   ├── components/
│   │   │   └── Layout.tsx
|   |   |   └── TaskFormFields.tsx
│   │   │
│   │   ├── pages/
│   │   │   ├── Login.tsx
│   │   │   ├── Register.tsx
│   │   │
│   │   ├── css/
│   │   │   ├── auth.css
│   │   │   
│   │   │
│   │   ├── App.tsx
│   │   ├── main.tsx
│   │   ├── api.ts
│   │   ├── auth.tsx
│   │   ├── types.ts
│   │   └── styles.css
│   │
│   ├── package.json
│   ├── package-lock.json
│   ├── tsconfig.json
│   ├── vite.config.ts
│   └── index.html
│
├── .gitignore
└── README.md

## ▶️ Run Project

### Backend

```bash
cd backend/TaskManagement.Api
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend

```bash
cd frontend
npm install
npm run dev
```

> Configure your SQL Server connection in `appsettings.json` before running the backend.

---

### 👨‍💻 Author

**Hafiz Syed Minhal Ali**
**cohort-9-dotnet-8172-hafiz**
