namespace Dal.Events;

public class UserUpdateFailed
{
    public Guid UserId { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }

}