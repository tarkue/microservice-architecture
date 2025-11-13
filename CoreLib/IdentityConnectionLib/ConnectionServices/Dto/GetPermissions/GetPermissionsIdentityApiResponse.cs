using Core.Entities;
using Core.Enums;

namespace IdentityConnectionLib.ConnectionServices.Dto.GetPermissions;

public class GetPermissionsIdentityApiResponse: IPermission
{
    public Guid Id { get; init; }
    public PermissionType Type { get; init; }
    public Guid? ChatId { get; init; }
    public Guid? ResourceId { get; init; }
}