using Domain.Entities.Base;

namespace Domain.Entities;



public interface IMessage: IBaseEntity<Guid>
{
    string Content { get; init; }
    IResource[] Attachment {  get; init; }
    DateTime Created { get; init; }
}