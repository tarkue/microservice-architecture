using Domain.Entities.Base;

namespace Domain.Entities;

public interface IChat: IBaseEntity<Guid>
{
    string Name { get; init; }
    short UnreadMessagesCount { get; init; }
    IResource Photo { get; init; }
}