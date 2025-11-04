using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities;

public interface IResource: IBaseEntity<Guid>
{
    string Source { get; init; }
    ResourceType Type { get; init; }
}