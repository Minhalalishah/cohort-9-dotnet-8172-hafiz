using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskStatus = TaskManagement.Api.Models.TaskStatus;

namespace TaskManagement.Api.Services;

public class TaskService(AppDbContext db, ILogger<TaskService> logger, ActivityLogService activity)
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

        // The task itself is already saved at this point. Everything below is
        // "nice to have" (loading the assignee's name, writing an activity log
        // entry) - if either of these fails for any reason, we don't want the
        // whole request to blow up and report an error for a task that was, in
        // fact, created successfully.
        try
        {
            await db.Entry(item).Reference(x => x.AssignedTo).LoadAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to load AssignedTo for task {TaskId}", item.Id);
        }

        try
        {
            await activity.LogAsync(item.Id, userId, "Created", $"Task \"{item.Title}\" created.");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to write activity log for task {TaskId}", item.Id);
        }

        return Map(item);
    }

    public async Task<TaskResponse?> UpdateAsync(int id, int userId, bool isAdmin, UpdateTaskRequest r)
    {
        var item = await db.Tasks.Include(x => x.AssignedTo).FirstOrDefaultAsync(x => x.Id == id);
        if (item is null || (!isAdmin && item.CreatedById != userId && item.AssignedToId != userId)) return null;

        var changes = DescribeChanges(item, r);

        item.Title = r.Title.Trim(); item.Description = r.Description.Trim(); item.Status = r.Status;
        item.Priority = r.Priority; item.Category = r.Category.Trim(); item.DueDate = r.DueDate;
        item.AssignedToId = r.AssignedToId; item.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        logger.LogInformation("Task updated: {TaskId} by {UserId}", id, userId);

        if (changes.Count > 0)
        {
            try
            {
                await activity.LogAsync(id, userId, "Updated", string.Join(" ", changes));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to write activity log for task {TaskId}", id);
            }
        }

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

    private static List<string> DescribeChanges(TaskItem item, UpdateTaskRequest r)
    {
        var changes = new List<string>();
        if (item.Status != r.Status)
            changes.Add($"Status changed from {item.Status} to {r.Status}.");
        if (item.Priority != r.Priority)
            changes.Add($"Priority changed from {item.Priority} to {r.Priority}.");
        if (item.AssignedToId != r.AssignedToId)
            changes.Add("Assignee changed.");
        if (!string.Equals(item.Title, r.Title.Trim(), StringComparison.Ordinal))
            changes.Add("Title changed.");
        return changes;
    }
}
