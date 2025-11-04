using Core.Api.Interfaces;
using Core.Interfaces;
using Dal.Events;
using Dal.Models;
using IdentityService.Dtos.Requests;
using IdentityService.Dtos.Responses;
using Logic.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

    
namespace IdentityService.Controllers;

[ApiController]
public class UserController(
    IUserService userService, IPublishEndpoint publishEndpoint, 
    IHttpContextAccessor httpContextAccessor, ICurrentUser currentUser)
{
    [Authorize(Roles = "Admin")]
    [HttpGet("admin/user")]
    public async Task<IPaginatedResult<UserResponse>> GetAll(PaginatedWithSearchRequest paginatedWithSearchRequest)
    {
        var paginatedResult = await userService.GetAllAsync(
            paginatedWithSearchRequest.PageIndex,
            paginatedWithSearchRequest.PageSize,
            paginatedWithSearchRequest.Search);
        
        return UserResponse.FromDal(paginatedResult);
    }
    
    [Authorize(Roles = "User")]
    [HttpGet("user")]
    public async Task<UserResponse> GetCurrentUser()
    {
        var userDal = await GetCurrentUserOrThrow();
        return UserResponse.FromDal(userDal);
    }
    
    [Authorize(Roles = "User")]
    [HttpGet("user/{id:guid}")]
    public async Task<UserInfoResponse> GetUserInfoById(Guid id)
    {
        var currentUserId = currentUser.GetUserGuidOrThrow(httpContextAccessor.HttpContext?.User.Claims);
        try
        {
            var userDal = await userService.FindByIdForUser(currentUserId, id);
            return UserInfoResponse.FromDal(userDal);
        }
        catch (Exception ex)
        {
            throw new BadHttpRequestException(ex.Message, StatusCodes.Status403Forbidden);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("/admin/user/{id:guid}")]
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
    [HttpPatch("user")]
    public async Task UpdateUser([FromBody] UpdateUserRequest updateUserRequest)
    {
        var userDal = await GetCurrentUserOrThrow();
        httpContextAccessor.HttpContext?.Request.Headers.TryGetValue(
            "Authorization", out var authorization);
        var accessToken = authorization.First()!.Split(' ')[1];
        
        var startedEvent = new UserUpdateStarted
        {
            UserId = userDal.Id,
            AccessToken = accessToken,
            Name = updateUserRequest.Name,
            Email = updateUserRequest.Email,
            Photo = updateUserRequest.Photo,
        };
        
        await publishEndpoint.Publish(startedEvent);

        
    }

    private async Task<UserDal> GetCurrentUserOrThrow()
    {
        var id = currentUser.GetUserGuidOrThrow(httpContextAccessor.HttpContext?.User.Claims);
        var userDal = await userService.FindByIdAsync(id);

        return userDal ?? throw new BadHttpRequestException("User not found", StatusCodes.Status404NotFound);
    }
}