using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskStatus = TaskManagement.Api.Models.TaskStatus;

namespace TaskManagement.Api.Services;

public class DashboardService(AppDbContext db)
{
    public async Task<object> GetCountsAsync(int userId, bool isAdmin)
    {
        var q = db.Tasks.AsNoTracking().AsQueryable();
        if (!isAdmin) q = q.Where(x => x.AssignedToId == userId || x.CreatedById == userId);

        return new
        {
            pending = await q.CountAsync(x => x.Status == TaskStatus.Pending),
            inProgress = await q.CountAsync(x => x.Status == TaskStatus.InProgress),
            completed = await q.CountAsync(x => x.Status == TaskStatus.Completed),
            total = await q.CountAsync()
        };
    }
}
