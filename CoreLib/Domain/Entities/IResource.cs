using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public interface IResource: IBaseEntity<Guid>
{
    string Source { get; init; }
    ResourceType Type { get; init; }
}