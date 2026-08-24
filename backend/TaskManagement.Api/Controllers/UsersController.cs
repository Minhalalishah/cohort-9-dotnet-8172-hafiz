using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Controllers;

[ApiController, Authorize, Route("api/users")]
public class UsersController(AppDbContext db) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<UserResponse>> Me()
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await db.Users.FindAsync(id);
        return user is null ? NotFound() : Ok(new UserResponse(user.Id, user.FullName, user.Email, user.Role.ToString()));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IEnumerable<UserResponse>> All() =>
        await db.Users.AsNoTracking().Select(x => new UserResponse(x.Id, x.FullName, x.Email, x.Role.ToString())).ToListAsync();
}
