using Domain.Entities;
using Domain.Enums;

namespace IdentityService.Dtos.Requests;

public class CreatePermission: IPermission
{
    public Guid Id { get; init; }
    public PermissionType Type { get; init; }
    public Guid? ChatId { get; init; }
    public Guid? ResourceId { get; init; }
}