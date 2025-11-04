using Core.Entities;

namespace Dal.Events;



public class UserUpdateRequested
{
    public required Guid UserId { get; init; }
    public required UserUpdateData NewData { get; init; }
    public required UserUpdateData OldData { get; init; }
    public required DateTime Timestamp { get; init; }
    public required string AccessToken { get; init; }
}