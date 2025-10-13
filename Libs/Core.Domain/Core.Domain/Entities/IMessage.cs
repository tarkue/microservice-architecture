using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;



public interface IMessage: IBaseEntity<Guid>
{
    string Content { get; init; }
    IResource[] Attachment {  get; init; }
    DateTime Created { get; init; }
}