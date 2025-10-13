using IdentityService.Dtos.Requests;
using IdentityService.Dtos.Responses;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
[Route("admin/user/{id:guid}/permission")]
public class PermissionController(IPermissionService permissionService)
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<PermissionResponse[]> GetPermission(Guid id)
    {
        var permissionDal = await permissionService.FindByUserAsync(id);
        return PermissionResponse.FromDalEnumerate(permissionDal);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task AddPermission(Guid id, [FromBody] CreatePermissionRequest request)
    {
        await permissionService.CreateAsync(id, request);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{permissionId:guid}")]
    public async Task DeletePermission(Guid id, Guid permissionId)
    {
        await permissionService.DeleteAsync(id, permissionId);
    }
}