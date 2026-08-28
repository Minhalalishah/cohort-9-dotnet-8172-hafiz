using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using Xunit;

namespace TaskManagement.Tests;

public class TagServiceTests
{
    private static TagService CreateService(out AppDbContext db)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        db = new AppDbContext(options);
        return new TagService(db, NullLogger<TagService>.Instance);
    }

    [Fact]
    public async Task Create_DuplicateName_Throws()
    {
        var service = CreateService(out var db);
        await service.CreateAsync(new TagRequest("Urgent", "#ef4444"));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(new TagRequest("urgent", "#000000")));
        await db.DisposeAsync();
    }

    [Fact]
    public async Task Create_EmptyName_Throws()
    {
        var service = CreateService(out var db);
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(new TagRequest("  ", "#000000")));
        await db.DisposeAsync();
    }

    [Fact]
    public async Task AssignToTask_ThenGetAll_ReflectsTaskCount()
    {
        var service = CreateService(out var db);
        db.Users.Add(new User { Id = 1, FullName = "U", Email = "u@x.com", PasswordHash = "x" });
        var task = new TaskItem { Title = "T", Description = "D", Category = "General", CreatedById = 1 };
        db.Tasks.Add(task);
        await db.SaveChangesAsync();

        var tag = await service.CreateAsync(new TagRequest("Backend", "#3b82f6"));
        var assigned = await service.AssignToTaskAsync(task.Id, tag.Id);
        var all = await service.GetAllAsync();

        Assert.True(assigned);
        Assert.Equal(1, all.Single(t => t.Id == tag.Id).TaskCount);
        await db.DisposeAsync();
    }

    [Fact]
    public async Task RemoveFromTask_UnassignedTag_ReturnsFalse()
    {
        var service = CreateService(out var db);
        db.Users.Add(new User { Id = 1, FullName = "U", Email = "u@x.com", PasswordHash = "x" });
        var task = new TaskItem { Title = "T", Description = "D", Category = "General", CreatedById = 1 };
        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        var tag = await service.CreateAsync(new TagRequest("Backend", "#3b82f6"));

        var removed = await service.RemoveFromTaskAsync(task.Id, tag.Id);

        Assert.False(removed);
        await db.DisposeAsync();
    }
}
