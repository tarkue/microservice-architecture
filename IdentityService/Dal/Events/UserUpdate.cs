namespace Dal.Events;

public class UserUpdate
{
    public required Guid UserId { get; init; }
    public required bool Success { get; init; }
    public required DateTime Timestamp { get; init; }
    
    public required string? AccessToken { get; init; }
    public required UserUpdateData? OldData { get; init; }
}