using Api.Dtos;
using Core;
using Core.Api.Interfaces;
using Core.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/chat/{id:guid}/message")]
public class MessageController(ICurrentUser currentUser, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "User")]
    public PaginatedResult<Message> GetAll(Guid id, GetAllOptionsQuery options)
    {
        return new PaginatedResult<Message> { Data = [], Meta = new PaginatedResultMeta() };
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    public void Send(Guid id, [FromBody] IMessageCreate messageCreate)
    { }
    
    private Guid GetUserIdOrThrow() => currentUser.GetUserGuidOrThrow(httpContextAccessor.HttpContext?.User.Claims);
}