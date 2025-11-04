using Core.Entities;

namespace Dal.Events;

public class ChatWithUserUpdateRequested
{
    public Guid UserId { get; init; }
    public string? Name { get; init; }
    public Guid? Photo { get; init; }
}