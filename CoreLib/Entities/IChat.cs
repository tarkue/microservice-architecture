using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public interface IChat: IBaseEntity<Guid>
{
    string Name { get; init; }
    short UnreadMessagesCount { get; init; }
    IResource Photo { get; init; }
}