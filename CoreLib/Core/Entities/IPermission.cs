using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public interface ICreatePermission
{
    PermissionType Type { get; init; }
    Guid? ChatId { get; init; }
    Guid? ResourceId { get; init; }
}

public interface IPermission: ICreatePermission, IBaseEntity<Guid> {}