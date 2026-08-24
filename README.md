## 📊 Dashboard & Task Management Backend

## ✨ Features Added

### 📊 Dashboard

- Added Dashboard API functionality
- Added completed task count
- Added in-progress task count
- Added pending task count
- Added user-specific task statistics
- Added admin/super-user statistics for all tasks
- Added task status-based counting

### 📝 Task Management

- Implemented Task Controller
- Create new tasks
- Retrieve task list
- Retrieve task details by ID
- Update existing tasks
- Delete tasks
- Assign tasks to users
- Set task priority
- Set task category
- Set task due date
- Manage task status
- Added filtering support where applicable

### 🔐 Authorization

- Dashboard data is based on the authenticated user
- Regular users can access their own task information
- Admin/Super User can access overall task statistics
- Protected API endpoints using authentication/authorization

### 🗄️ Database

- Integrated Dashboard and Task functionality with Entity Framework Core
- Uses SQL Server database
- Added database queries for task status statistics
- Uses existing User and Task relationships

### 🛡️ Error Handling & Logging

- Uses existing global exception handling
- Integrated with Serilog logging
- Handles invalid task IDs and request errors appropriately

## 🧪 Testing

The following checks were performed:

- `dotnet restore`
- `dotnet build`
- SQL Server database connection
- Swagger API testing
- Dashboard API testing
- Task Create testing
- Task Read testing
- Task Update testing
- Task Delete testing
- Authorization testing

## 📁 Main Areas Changed

```text
backend/
└── TaskManagement.Api/
    ├── Controllers/
    │   ├── DashboardController.cs
    │   └── TasksController.cs
    │
    ├── Services/
    │   ├── DashboardService.cs
    │   └── TaskService.cs
    │
    ├── Models/
    │   └── Task-related models/DTOs
    │
    ├── Data/
    │   └── AppDbContext.cs
    │
    └── Program.cs
