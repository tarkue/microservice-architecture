using System.Security.Claims;
using Dal.Models;
using Domain.Interfaces;
using IdentityService.Dtos.Requests;
using IdentityService.Dtos.Responses;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

    
namespace IdentityService.Controllers;

[ApiController]
[Route("user")]
public class UserController(IUserService userService, IHttpContextAccessor httpContextAccessor )
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IPaginatedResult<UserResponse>> GetAll(PaginatedWithSearchRequest paginatedWithSearchRequest)
    {
        var paginatedResult = await userService.GetAllAsync(
            paginatedWithSearchRequest.PageIndex,
            paginatedWithSearchRequest.PageSize,
            paginatedWithSearchRequest.Search);
        
        return UserResponse.FromDal(paginatedResult);
    }
    
    [Authorize(Roles = "User")]
    [HttpGet("me")]
    public async Task<UserResponse> GetCurrentUser()
    {
        var userDal = await GetCurrentUserOrThrow();
        return UserResponse.FromDal(userDal);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("/{id:guid}")]
    public async Task<UserResponse> GetById(Guid id)
    {
        try
        {
            var user = await userService.GetByIdOrThrow(id);
            return UserResponse.FromDal(user);
        } 
        catch (ArgumentException) 
        {
            throw new BadHttpRequestException("User not found", StatusCodes.Status404NotFound);
        }
    }

    [Authorize(Roles = "User")]
    [HttpPatch]
    public async Task UpdateUser([FromBody] UpdateUserRequest updateUserRequest)
    {
        var userDal = await GetCurrentUserOrThrow();
        await userService.UpdateAsync(userDal, updateUserRequest);
    }

    private async Task<UserDal> GetCurrentUserOrThrow()
    {
        var id = GetGuidOrThrow();
        var userDal = await userService.FindByIdAsync(id);

        return userDal ?? throw new BadHttpRequestException("User not found", StatusCodes.Status404NotFound);
    }
    
    private Guid GetGuidOrThrow()
    {
        
        var idInString = httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type.Equals(ClaimTypes.Sid)).Value;
        
        if (!Guid.TryParse(idInString, out var id))
            throw new BadHttpRequestException("User not found", StatusCodes.Status404NotFound);
        
        return id;
    }
}