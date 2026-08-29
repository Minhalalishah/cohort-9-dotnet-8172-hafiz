using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Models;
using TaskStatus = TaskManagement.Api.Models.TaskStatus;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Controllers;

[ApiController, Authorize, Route("api/tasks")]
public class TasksController(TaskService service) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private bool IsAdmin => User.IsInRole("Admin");

    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> Get(
        [FromQuery] string? search, [FromQuery] TaskStatus? status, [FromQuery] TaskPriority? priority) =>
        Ok(await service.GetAsync(UserId, IsAdmin, search, status, priority));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await service.GetAsync(id, UserId, IsAdmin);
        return item is null ? NotFound(new { message = "Task not found." }) : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request) =>
        Ok(await service.CreateAsync(UserId, request));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTaskRequest request)
    {
        var item = await service.UpdateAsync(id, UserId, IsAdmin, request);
        return item is null ? NotFound(new { message = "Task not found or access denied." }) : Ok(item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await service.DeleteAsync(id, UserId, IsAdmin);
        return ok ? NoContent() : NotFound(new { message = "Task not found or access denied." });
    }

    [HttpGet("{id:int}/activity")]
    public async Task<IActionResult> GetActivity(int id, [FromServices] ActivityLogService activityLog)
    {
        var logs = await activityLog.GetForTaskAsync(id, UserId, IsAdmin);
        return logs is null ? NotFound(new { message = "Task not found or access denied." }) : Ok(logs);
    }
}
