using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Services;

public class ActivityLogService(AppDbContext db)
{
    public async Task LogAsync(int taskId, int userId, string action, string details = "")
    {
        db.TaskActivityLogs.Add(new TaskActivityLog
        {
            TaskItemId = taskId,
            UserId = userId,
            Action = action,
            Details = details
        });
        await db.SaveChangesAsync();
    }

    public async Task<List<ActivityLogResponse>?> GetForTaskAsync(int taskId, int userId, bool isAdmin)
    {
        var task = await db.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == taskId);
        if (task is null || (!isAdmin && task.CreatedById != userId && task.AssignedToId != userId))
            return null;

        return await db.TaskActivityLogs
            .Include(l => l.User)
            .AsNoTracking()
            .Where(l => l.TaskItemId == taskId)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new ActivityLogResponse(l.Id, l.TaskItemId, l.UserId, l.User.FullName, l.Action, l.Details, l.CreatedAt))
            .ToListAsync();
    }
}
