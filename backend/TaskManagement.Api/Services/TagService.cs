using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Services;

public class TagService(AppDbContext db, ILogger<TagService> logger)
{
    public async Task<List<TagResponse>> GetAllAsync() =>
        await db.Tags
            .Include(t => t.Tasks)
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new TagResponse(t.Id, t.Name, t.Color, t.Tasks.Count))
            .ToListAsync();

    public async Task<TagResponse> CreateAsync(TagRequest request)
    {
        var name = request.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tag name cannot be empty.");
        if (await db.Tags.AnyAsync(t => t.Name.ToLower() == name.ToLower()))
            throw new InvalidOperationException($"Tag '{name}' already exists.");

        var tag = new Tag
        {
            Name = name,
            Color = string.IsNullOrWhiteSpace(request.Color) ? "#6b7280" : request.Color
        };
        db.Tags.Add(tag);
        await db.SaveChangesAsync();

        logger.LogInformation("Tag created: {TagId} ({Name})", tag.Id, tag.Name);
        return new TagResponse(tag.Id, tag.Name, tag.Color, 0);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tag = await db.Tags.FirstOrDefaultAsync(t => t.Id == id);
        if (tag is null) return false;

        db.Tags.Remove(tag);
        await db.SaveChangesAsync();
        logger.LogInformation("Tag deleted: {TagId}", id);
        return true;
    }

    public async Task<bool> AssignToTaskAsync(int taskId, int tagId)
    {
        var task = await db.Tasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
        var tag = await db.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
        if (task is null || tag is null) return false;

        if (task.Tags.All(t => t.Id != tagId))
        {
            task.Tags.Add(tag);
            await db.SaveChangesAsync();
            logger.LogInformation("Tag {TagId} assigned to task {TaskId}", tagId, taskId);
        }
        return true;
    }

    public async Task<bool> RemoveFromTaskAsync(int taskId, int tagId)
    {
        var task = await db.Tasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
        var tag = task?.Tags.FirstOrDefault(t => t.Id == tagId);
        if (task is null || tag is null) return false;

        task.Tags.Remove(tag);
        await db.SaveChangesAsync();
        logger.LogInformation("Tag {TagId} removed from task {TaskId}", tagId, taskId);
        return true;
    }
}
