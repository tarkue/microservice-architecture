using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public interface IResource: IBaseEntity<Guid>
{
    string Source { get; init; }
    ResourceType Type { get; init; }
}