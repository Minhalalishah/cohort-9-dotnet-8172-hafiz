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

public record CommentRequest(string Content);
public record CommentResponse(int Id, int TaskItemId, int UserId, string UserName, string Content, DateTime CreatedAt);

public record TagRequest(string Name, string Color);
public record TagResponse(int Id, string Name, string Color, int TaskCount);

public record ActivityLogResponse(
    int Id, int TaskItemId, int UserId, string UserName, string Action, string Details, DateTime CreatedAt);
