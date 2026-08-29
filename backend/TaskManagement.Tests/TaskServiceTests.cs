using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using Xunit;

using ApiTaskStatus = TaskManagement.Api.Models.TaskStatus;

namespace TaskManagement.Tests;

public class TaskServiceTests
{
    [Fact]
    public async Task Create_ThenGet_ReturnsTask()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new AppDbContext(options);

        db.Users.Add(new User
        {
            Id = 1,
            FullName = "User",
            Email = "u@x.com",
            PasswordHash = "x"
        });

        await db.SaveChangesAsync();

        var logger = LoggerFactory
            .Create(builder => { })
            .CreateLogger<TaskService>();

        var service = new TaskService(db, logger);

        var created = await service.CreateAsync(
            1,
            new CreateTaskRequest(
                "Test task",
                "Description",
                ApiTaskStatus.Pending,
                TaskPriority.High,
                "Work",
                null,
                1
            )
        );

        var fetched = await service.GetAsync(
            created.Id,
            1,
            false
        );

        Assert.NotNull(fetched);
        Assert.Equal("Test task", fetched!.Title);
        Assert.Equal(ApiTaskStatus.Pending, fetched.Status);
        Assert.Equal(TaskPriority.High, fetched.Priority);
    }
}