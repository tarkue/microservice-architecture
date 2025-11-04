namespace Dal.Events;

public class UpdateUserCompleted
{
    public Guid UserId { get; init; }
    public DateTime Timestamp { get; init; }
}