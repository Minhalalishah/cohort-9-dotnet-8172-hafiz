using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using Xunit;

namespace TaskManagement.Tests;

public class CommentServiceTests
{
    private static async Task<(AppDbContext Db, CommentService Service, int TaskId)> CreateAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var db = new AppDbContext(options);
        db.Users.Add(new User { Id = 1, FullName = "Owner", Email = "owner@x.com", PasswordHash = "x" });
        db.Users.Add(new User { Id = 2, FullName = "Stranger", Email = "stranger@x.com", PasswordHash = "x" });
        var task = new TaskItem { Title = "Task", Description = "Desc", Category = "General", CreatedById = 1 };
        db.Tasks.Add(task);
        await db.SaveChangesAsync();

        var service = new CommentService(db, new ActivityLogService(db), NullLogger<CommentService>.Instance);
        return (db, service, task.Id);
    }

    [Fact]
    public async Task Add_ByOwner_Succeeds()
    {
        var (db, service, taskId) = await CreateAsync();
        var comment = await service.AddAsync(taskId, 1, false, new CommentRequest("Looks good."));

        Assert.NotNull(comment);
        Assert.Equal("Looks good.", comment!.Content);
        Assert.Equal("Owner", comment.UserName);
        await db.DisposeAsync();
    }

    [Fact]
    public async Task Add_ByUserWithoutAccess_ReturnsNull()
    {
        var (db, service, taskId) = await CreateAsync();
        var comment = await service.AddAsync(taskId, 2, false, new CommentRequest("Snooping."));

        Assert.Null(comment);
        await db.DisposeAsync();
    }

    [Fact]
    public async Task Add_EmptyContent_Throws()
    {
        var (db, service, taskId) = await CreateAsync();
        await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(taskId, 1, false, new CommentRequest("   ")));
        await db.DisposeAsync();
    }

    [Fact]
    public async Task Delete_ByDifferentUser_ReturnsFalse()
    {
        var (db, service, taskId) = await CreateAsync();
        var comment = await service.AddAsync(taskId, 1, false, new CommentRequest("Mine."));
        var ok = await service.DeleteAsync(comment!.Id, 2, false);

        Assert.False(ok);
        await db.DisposeAsync();
    }

    [Fact]
    public async Task Delete_ByAdmin_Succeeds()
    {
        var (db, service, taskId) = await CreateAsync();
        var comment = await service.AddAsync(taskId, 1, false, new CommentRequest("Mine."));
        var ok = await service.DeleteAsync(comment!.Id, 2, true);

        Assert.True(ok);
        await db.DisposeAsync();
    }
}
