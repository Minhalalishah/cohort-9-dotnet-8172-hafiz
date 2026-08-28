using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Controllers;

[ApiController, Authorize, Route("api/tags")]
public class TagsController(TagService service) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TagResponse>> Get() => await service.GetAllAsync();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(TagRequest request)
    {
        try { return Ok(await service.CreateAsync(request)); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await service.DeleteAsync(id);
        return ok ? NoContent() : NotFound(new { message = "Tag not found." });
    }

    [HttpPost("~/api/tasks/{taskId:int}/tags/{tagId:int}")]
    public async Task<IActionResult> AssignToTask(int taskId, int tagId)
    {
        var ok = await service.AssignToTaskAsync(taskId, tagId);
        return ok ? NoContent() : NotFound(new { message = "Task or tag not found." });
    }

    [HttpDelete("~/api/tasks/{taskId:int}/tags/{tagId:int}")]
    public async Task<IActionResult> RemoveFromTask(int taskId, int tagId)
    {
        var ok = await service.RemoveFromTaskAsync(taskId, tagId);
        return ok ? NoContent() : NotFound(new { message = "Task or tag not found." });
    }
}
