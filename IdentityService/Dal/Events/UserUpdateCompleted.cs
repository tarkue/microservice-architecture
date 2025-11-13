namespace Dal.Events;

public class UserUpdateCompleted
{
    public Guid UserId { get; init; }
    public DateTime Timestamp { get; init; }
}