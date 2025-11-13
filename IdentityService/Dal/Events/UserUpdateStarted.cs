using Core.Entities;

namespace Dal.Events;

public class UserUpdateStarted: UserUpdateData
{
    public required Guid UserId { get; init; }
    public required string AccessToken { get; init; }
}