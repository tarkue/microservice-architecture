using Core.Entities;
using Core.Enums;

namespace IdentityService.Dtos.Requests;

public class CreatePermissionRequest
    : ICreatePermission
{
    public PermissionType Type { get; init; }
    public Guid? ChatId { get; init; }
    public Guid? ResourceId { get; init; }
}