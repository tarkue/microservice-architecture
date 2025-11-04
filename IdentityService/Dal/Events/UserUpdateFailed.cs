namespace Dal.Events;

public class UserUpdateFailed
{
    public required Guid UserId { get; init; }
    public required string Reason { get; init; } = string.Empty;
    public required DateTime Timestamp { get; init; }
}