using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Services;

public class AuthService(AppDbContext db, JwtTokenService jwt, ILogger<AuthService> logger)
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(x => x.Email == email))
            throw new InvalidOperationException("Email is already registered.");

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.User
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();
        logger.LogInformation("New user registered: {UserId}", user.Id);

        return new AuthResponse(jwt.Create(user), user.Id, user.FullName, user.Email, user.Role.ToString());
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        logger.LogInformation("User logged in: {UserId}", user.Id);
        return new AuthResponse(jwt.Create(user), user.Id, user.FullName, user.Email, user.Role.ToString());
    }
}
