using Core.Entities.Base;

namespace Core.Entities;

public interface IChat: IBaseEntity<Guid>
{
    string Name { get; init; }
    short UnreadMessagesCount { get; init; }
}

public interface IUpdateChat
{
    public string? Name  { get; init; }
    public Guid? Photo { get; init; }
}