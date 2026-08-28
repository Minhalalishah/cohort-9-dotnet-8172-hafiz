using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Models;
using TaskStatus = TaskManagement.Api.Models.TaskStatus;

namespace TaskManagement.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync()) return;

        var admin = new User
        {
            FullName = "System Admin",
            Email = "admin@tasktool.local",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Admin
        };
        var user = new User
        {
            FullName = "Regular User",
            Email = "user@tasktool.local",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
            Role = UserRole.User
        };

        db.Users.AddRange(admin, user);
        await db.SaveChangesAsync();

        var urgentTag = new Tag { Name = "Urgent", Color = "#ef4444" };
        var backendTag = new Tag { Name = "Backend", Color = "#3b82f6" };
        db.Tags.AddRange(urgentTag, backendTag);

        var welcomeTask = new TaskItem
        {
            Title = "Welcome task",
            Description = "Update this task or create your own.",
            Status = TaskStatus.Pending,
            Priority = TaskPriority.Medium,
            Category = "General",
            DueDate = DateTime.UtcNow.AddDays(7),
            CreatedById = admin.Id,
            AssignedToId = user.Id,
            Tags = [urgentTag]
        };
        db.Tasks.Add(welcomeTask);
        await db.SaveChangesAsync();

        db.Comments.Add(new Comment
        {
            TaskItemId = welcomeTask.Id,
            UserId = admin.Id,
            Content = "Let me know if you have questions about this one."
        });
        db.TaskActivityLogs.Add(new TaskActivityLog
        {
            TaskItemId = welcomeTask.Id,
            UserId = admin.Id,
            Action = "Created",
            Details = $"Task \"{welcomeTask.Title}\" created."
        });
        await db.SaveChangesAsync();
    }
}
