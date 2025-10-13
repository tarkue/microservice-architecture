using Dal.Models;
using Domain.Entities;
using Domain.Enums;

namespace IdentityService.Dtos.Responses;

public class PermissionResponse: IPermission
{
    public required Guid Id { get; init; }
    public required PermissionType Type { get; init; }
    public Guid? ChatId { get; init; }
    public Guid? ResourceId { get; init; }
    
    public static PermissionResponse FromDal(PermissionDal permissionDal)
    {
        return new PermissionResponse()
        {
            Id = permissionDal.Id,
            Type = permissionDal.Type,
            ChatId = permissionDal.ChatId,
            ResourceId = permissionDal.ResourceId,
        };
    }

    public static PermissionResponse[] FromDalEnumerate(IEnumerable<PermissionDal> permissionDals)
    {
        return permissionDals.Select(permissionDal => PermissionResponse.FromDal(permissionDal)).ToArray();
    }
}