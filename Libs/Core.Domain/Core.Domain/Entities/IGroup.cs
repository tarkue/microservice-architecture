using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public interface IGroup: IBaseEntity<Guid>
{
    Guid[] UserIds { get; init; }
}