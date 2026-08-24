using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskStatus = TaskManagement.Api.Models.TaskStatus;

namespace TaskManagement.Api.Services;

public class TaskService(AppDbContext db, ILogger<TaskService> logger)
{
    private static TaskResponse Map(TaskItem x) =>
        new(x.Id, x.Title, x.Description, x.Status, x.Priority, x.Category, x.DueDate,
            x.CreatedById, x.AssignedToId, x.AssignedTo?.FullName, x.CreatedAt, x.UpdatedAt);

    public async Task<List<TaskResponse>> GetAsync(int userId, bool isAdmin, string? search, TaskStatus? status, TaskPriority? priority)
    {
        var query = db.Tasks.Include(x => x.AssignedTo).AsNoTracking().AsQueryable();
        if (!isAdmin)
            query = query.Where(x => x.AssignedToId == userId || x.CreatedById == userId);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Title.Contains(search) || x.Description.Contains(search) || x.Category.Contains(search));
        if (status.HasValue) query = query.Where(x => x.Status == status.Value);
        if (priority.HasValue) query = query.Where(x => x.Priority == priority.Value);
        return await query.OrderByDescending(x => x.CreatedAt).Select(x => Map(x)).ToListAsync();
    }

    public async Task<TaskResponse?> GetAsync(int id, int userId, bool isAdmin)
    {
        var x = await db.Tasks.Include(t => t.AssignedTo).AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (x is null || (!isAdmin && x.AssignedToId != userId && x.CreatedById != userId)) return null;
        return Map(x);
    }

    public async Task<TaskResponse> CreateAsync(int userId, CreateTaskRequest r)
    {
        var item = new TaskItem
        {
            Title = r.Title.Trim(), Description = r.Description.Trim(), Status = r.Status,
            Priority = r.Priority, Category = r.Category.Trim(), DueDate = r.DueDate,
            CreatedById = userId, AssignedToId = r.AssignedToId
        };
        db.Tasks.Add(item);
        await db.SaveChangesAsync();
        logger.LogInformation("Task created: {TaskId} by {UserId}", item.Id, userId);
        await db.Entry(item).Reference(x => x.AssignedTo).LoadAsync();
        return Map(item);
    }

    public async Task<TaskResponse?> UpdateAsync(int id, int userId, bool isAdmin, UpdateTaskRequest r)
    {
        var item = await db.Tasks.Include(x => x.AssignedTo).FirstOrDefaultAsync(x => x.Id == id);
        if (item is null || (!isAdmin && item.CreatedById != userId && item.AssignedToId != userId)) return null;

        item.Title = r.Title.Trim(); item.Description = r.Description.Trim(); item.Status = r.Status;
        item.Priority = r.Priority; item.Category = r.Category.Trim(); item.DueDate = r.DueDate;
        item.AssignedToId = r.AssignedToId; item.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        logger.LogInformation("Task updated: {TaskId} by {UserId}", id, userId);
        return Map(item);
    }

    public async Task<bool> DeleteAsync(int id, int userId, bool isAdmin)
    {
        var item = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null || (!isAdmin && item.CreatedById != userId)) return false;
        db.Tasks.Remove(item);
        await db.SaveChangesAsync();
        logger.LogInformation("Task deleted: {TaskId} by {UserId}", id, userId);
        return true;
    }
}
