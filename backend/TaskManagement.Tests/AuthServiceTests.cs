using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using Xunit;

namespace TaskManagement.Tests;

public class AuthServiceTests
{
    private static (AppDbContext Db, AuthService Service) Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var db = new AppDbContext(options);
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string,string?>
        {
            ["Jwt:Key"] = "this-is-a-test-secret-that-is-long-enough-123456",
            ["Jwt:Issuer"] = "test",
            ["Jwt:Audience"] = "test",
            ["Jwt:ExpiresMinutes"] = "60"
        }).Build();
        var jwt = new JwtTokenService(config);
        return (db, new AuthService(db, jwt, NullLogger<AuthService>.Instance));
    }

    [Fact]
    public async Task Register_Creates_User()
    {
        var (_, service) = Create();
        var result = await service.RegisterAsync(new RegisterRequest("Test User", "TEST@example.com", "Password@123"));
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("User", result.Role);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsNull()
    {
        var (db, service) = Create();
        await service.RegisterAsync(new RegisterRequest("Test", "a@b.com", "Correct@123"));
        var result = await service.LoginAsync(new LoginRequest("a@b.com", "Wrong@123"));
        Assert.Null(result);
        await db.DisposeAsync();
    }
}
