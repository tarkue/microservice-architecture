using Core.Api.Interfaces;
using IdentityService.Dtos.Requests;
using IdentityService.Dtos.Responses;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
public class PermissionController(IPermissionService permissionService, ICurrentUser currentUser, IHttpContextAccessor accessor)
{
    [Authorize(Roles = "User")]
    [HttpGet("user/permission")]
    public async Task<PermissionResponse[]> GetUserPermissions()
    {
        var userId = currentUser.GetUserGuidOrThrow(accessor.HttpContext?.User.Claims);
        return await this.GetPermission(userId);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("admin/user/{id:guid}/permission")]
    public async Task<PermissionResponse[]> GetPermission(Guid id)
    {
        var permissionDal = await permissionService.FindByUserAsync(id);
        return PermissionResponse.FromDalEnumerate(permissionDal);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/user/{id:guid}/permission")]
    public async Task AddPermission(Guid id, [FromBody] CreatePermissionRequest request)
    {
        await permissionService.CreateAsync(id, request);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("admin/user/{id:guid}/permission/{permissionId:guid}")]
    public async Task DeletePermission(Guid id, Guid permissionId)
    {
        await permissionService.DeleteAsync(id, permissionId);
    }
}