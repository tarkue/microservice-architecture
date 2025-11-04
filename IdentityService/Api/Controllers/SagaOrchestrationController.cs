using Core.Api.Interfaces;
using Dal.Events;
using Dal.Models;
using IdentityService.Dtos.Requests;
using IdentityService.Dtos.Responses;
using Logic.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
[Route("orchestration")]
public class SagaOrchestrationController(
    IHttpContextAccessor httpContextAccessor, 
    IUserService userService, IPublishEndpoint publishEndpoint, 
    IRequestClient<UserUpdateStarted> requestClient, ICurrentUser currentUser)
{
    [Authorize(Roles = "User")]
    [HttpPost("start-orchestration")]
    public async Task<ActionResult<Guid>> StartOrchestration([FromBody] UpdateUserRequest updateUserRequest)
    {
        var userDal = await GetCurrentUserOrThrow();
        var startedEvent = GetStatedEvent(userDal, updateUserRequest);

        await publishEndpoint.Publish(startedEvent);

        return new ActionResult<Guid>(userDal.Id);
    }

    [Authorize(Roles = "User")]
    [HttpPost("start-orchestration-request")]
    public async Task<ActionResult<Guid>> StartOrchestrationWithRequest([FromBody] UpdateUserRequest updateUserRequest)
    {
        var userDal = await GetCurrentUserOrThrow();
        var startedEvent = GetStatedEvent(userDal, updateUserRequest);
        
        var response = await requestClient.GetResponse<UserUpdateResponse>(startedEvent);
        return new ActionResult<Guid>(userDal.Id);
    }

    private UserUpdateStarted GetStatedEvent(UserDal userDal, UpdateUserRequest updateUserRequest)
    {
        httpContextAccessor.HttpContext?.Request.Headers.TryGetValue(
            "Authorization", out var authorization);
        var accessToken = authorization.First()!.Split(' ')[1];

        return new UserUpdateStarted
        {
            UserId = userDal.Id,
            AccessToken = accessToken,
            Name = updateUserRequest.Name,
            Email = updateUserRequest.Email,
            Photo = updateUserRequest.Photo,
        };
    }
    
    private async Task<UserDal> GetCurrentUserOrThrow()
    {
        var id = currentUser.GetUserGuidOrThrow(httpContextAccessor.HttpContext?.User.Claims);
        var userDal = await userService.FindByIdAsync(id);

        return userDal ?? throw new BadHttpRequestException("User not found", StatusCodes.Status404NotFound);
    }
}