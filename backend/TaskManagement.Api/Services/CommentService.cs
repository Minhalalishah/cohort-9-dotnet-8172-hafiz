using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Services;

public class CommentService(AppDbContext db, ActivityLogService activity, ILogger<CommentService> logger)
{
    private static CommentResponse Map(Comment c) =>
        new(c.Id, c.TaskItemId, c.UserId, c.User.FullName, c.Content, c.CreatedAt);

    private static bool CanAccess(TaskItem task, int userId, bool isAdmin) =>
        isAdmin || task.CreatedById == userId || task.AssignedToId == userId;

    public async Task<List<CommentResponse>?> GetForTaskAsync(int taskId, int userId, bool isAdmin)
    {
        var task = await db.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == taskId);
        if (task is null || !CanAccess(task, userId, isAdmin)) return null;

        return await db.Comments
            .Include(c => c.User)
            .AsNoTracking()
            .Where(c => c.TaskItemId == taskId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => Map(c))
            .ToListAsync();
    }

    public async Task<CommentResponse?> AddAsync(int taskId, int userId, bool isAdmin, CommentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
            throw new ArgumentException("Comment content cannot be empty.");
        if (request.Content.Length > 2000)
            throw new ArgumentException("Comment content cannot exceed 2000 characters.");

        var task = await db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task is null || !CanAccess(task, userId, isAdmin)) return null;

        var comment = new Comment { TaskItemId = taskId, UserId = userId, Content = request.Content.Trim() };
        db.Comments.Add(comment);
        await db.SaveChangesAsync();
        await db.Entry(comment).Reference(c => c.User).LoadAsync();

        logger.LogInformation("Comment {CommentId} added to task {TaskId} by {UserId}", comment.Id, taskId, userId);
        await activity.LogAsync(taskId, userId, "Commented", "Added a comment.");

        return Map(comment);
    }

    public async Task<bool> DeleteAsync(int commentId, int userId, bool isAdmin)
    {
        var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment is null || (!isAdmin && comment.UserId != userId)) return false;

        db.Comments.Remove(comment);
        await db.SaveChangesAsync();
        logger.LogInformation("Comment deleted: {CommentId} by {UserId}", commentId, userId);
        return true;
    }
}
