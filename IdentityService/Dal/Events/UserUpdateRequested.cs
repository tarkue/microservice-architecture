using Core.Entities;

namespace Dal.Events;

public class UserUpdateData : IUserUpdate
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public Guid? Photo { get; init; }
}

public class UserUpdateRequested
{
    public Guid UserId { get; init; }
    public UserUpdateData? NewData { get; init; }
    public UserUpdateData? OldData { get; init; }

}