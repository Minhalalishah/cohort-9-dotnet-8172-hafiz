using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Controllers;

[ApiController, Authorize, Route("api/tasks/{taskId:int}/comments")]
public class CommentsController(CommentService service) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private bool IsAdmin => User.IsInRole("Admin");

    [HttpGet]
    public async Task<IActionResult> Get(int taskId)
    {
        var comments = await service.GetForTaskAsync(taskId, UserId, IsAdmin);
        return comments is null
            ? NotFound(new { message = "Task not found or access denied." })
            : Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int taskId, CommentRequest request)
    {
        try
        {
            var comment = await service.AddAsync(taskId, UserId, IsAdmin, request);
            return comment is null
                ? NotFound(new { message = "Task not found or access denied." })
                : Ok(comment);
        }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> Delete(int taskId, int commentId)
    {
        var ok = await service.DeleteAsync(commentId, UserId, IsAdmin);
        return ok ? NoContent() : NotFound(new { message = "Comment not found or access denied." });
    }
}
