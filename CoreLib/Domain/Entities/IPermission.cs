using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public interface IPermission: IBaseEntity<Guid>
{
    PermissionType Type { get; init; }
    Guid? ChatId { get; init; }
    Guid? ResourceId { get; init; }
}
