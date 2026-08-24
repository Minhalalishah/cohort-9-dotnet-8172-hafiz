using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Controllers;

[ApiController, Authorize, Route("api/dashboard")]
public class DashboardController(DashboardService service) : ControllerBase
{
    [HttpGet("counts")]
    public async Task<IActionResult> Counts()
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var admin = User.IsInRole("Admin");
        return Ok(await service.GetCountsAsync(id, admin));
    }
}
