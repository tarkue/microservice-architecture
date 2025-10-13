using Core.Entities.Base;

namespace Core.Entities;

public interface IMessageCreate
{
    string Content { get; init; }
    IResource[]? Attachment {  get; init; }
    DateTime CreatedAt { get; init; }
}

public interface IMessage: IMessageCreate, IBaseEntity<Guid> { }
