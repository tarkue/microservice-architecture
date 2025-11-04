namespace Dal.Events;

public class ChatWithUserUpdate
{
    public Guid UserId { get; init; }
    public bool Success { get; init; }
    public DateTime Timestamp { get; init; }
    
    public required UserUpdateData? OldData { get; init; }
}