namespace TaskManagement.Api.Models;

public record RegisterRequest(string FullName, string Email, string Password);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, int UserId, string FullName, string Email, string Role);

public record CreateTaskRequest(
    string Title, string Description, TaskStatus Status, TaskPriority Priority,
    string Category, DateTime? DueDate, int? AssignedToId);

public record UpdateTaskRequest(
    string Title, string Description, TaskStatus Status, TaskPriority Priority,
    string Category, DateTime? DueDate, int? AssignedToId);

public record TaskResponse(
    int Id, string Title, string Description, TaskStatus Status, TaskPriority Priority,
    string Category, DateTime? DueDate, int CreatedById, int? AssignedToId,
    string? AssignedToName, DateTime CreatedAt, DateTime UpdatedAt);

public record UserResponse(int Id, string FullName, string Email, string Role);
