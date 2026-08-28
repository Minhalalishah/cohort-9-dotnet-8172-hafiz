using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using Xunit;

namespace TaskManagement.Tests;

public class TaskServiceTests
{
    [Fact]
    public async Task Create_ThenGet_ReturnsTask()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        await using var db = new AppDbContext(options);
        db.Users.Add(new User { Id = 1, FullName = "User", Email = "u@x.com", PasswordHash = "x" });
        await db.SaveChangesAsync();

        var service = new TaskService(db, NullLogger<TaskService>.Instance, new ActivityLogService(db));
        var created = await service.CreateAsync(1, new CreateTaskRequest(
            "Test task", "Description", TaskStatus.Pending, TaskPriority.High, "Work", null, 1));

        var fetched = await service.GetAsync(created.Id, 1, false);
        Assert.NotNull(fetched);
        Assert.Equal("Test task", fetched!.Title);
    }

    [Fact]
    public async Task Create_LogsActivity()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        await using var db = new AppDbContext(options);
        db.Users.Add(new User { Id = 1, FullName = "User", Email = "u@x.com", PasswordHash = "x" });
        await db.SaveChangesAsync();

        var activityLog = new ActivityLogService(db);
        var service = new TaskService(db, NullLogger<TaskService>.Instance, activityLog);
        var created = await service.CreateAsync(1, new CreateTaskRequest(
            "Logged task", "Description", TaskStatus.Pending, TaskPriority.Low, "Work", null, null));

        var logs = await activityLog.GetForTaskAsync(created.Id, 1, false);
        Assert.NotNull(logs);
        Assert.Single(logs!);
        Assert.Equal("Created", logs![0].Action);
    }

    [Fact]
    public async Task Update_StatusChange_LogsActivity()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        await using var db = new AppDbContext(options);
        db.Users.Add(new User { Id = 1, FullName = "User", Email = "u@x.com", PasswordHash = "x" });
        await db.SaveChangesAsync();

        var activityLog = new ActivityLogService(db);
        var service = new TaskService(db, NullLogger<TaskService>.Instance, activityLog);
        var created = await service.CreateAsync(1, new CreateTaskRequest(
            "Task", "Description", TaskStatus.Pending, TaskPriority.Low, "Work", null, null));

        await service.UpdateAsync(created.Id, 1, false, new UpdateTaskRequest(
            "Task", "Description", TaskStatus.Completed, TaskPriority.Low, "Work", null, null));

        var logs = await activityLog.GetForTaskAsync(created.Id, 1, false);
        Assert.NotNull(logs);
        Assert.Contains(logs!, l => l.Action == "Updated" && l.Details.Contains("Status changed"));
    }

    [Fact]
    public async Task Update_NonOwnerNonAdmin_ReturnsNull()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        await using var db = new AppDbContext(options);
        db.Users.Add(new User { Id = 1, FullName = "Owner", Email = "o@x.com", PasswordHash = "x" });
        db.Users.Add(new User { Id = 2, FullName = "Other", Email = "ot@x.com", PasswordHash = "x" });
        await db.SaveChangesAsync();

        var service = new TaskService(db, NullLogger<TaskService>.Instance, new ActivityLogService(db));
        var created = await service.CreateAsync(1, new CreateTaskRequest(
            "Task", "Description", TaskStatus.Pending, TaskPriority.Low, "Work", null, null));

        var result = await service.UpdateAsync(created.Id, 2, false, new UpdateTaskRequest(
            "Changed", "Description", TaskStatus.Completed, TaskPriority.Low, "Work", null, null));

        Assert.Null(result);
    }
}
