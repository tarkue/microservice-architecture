using Domain.Entities.Base;

namespace Domain.Entities;

public interface IGroup: IBaseEntity<Guid>
{
    Guid[] UserIds { get; init; }
}