using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities;

public interface ICreatePermission
{
    PermissionType Type { get; init; }
    Guid? ChatId { get; init; }
    Guid? ResourceId { get; init; }
}

public interface IPermission: ICreatePermission, IBaseEntity<Guid> {}