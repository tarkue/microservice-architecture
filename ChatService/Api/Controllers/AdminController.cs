using Api.Dtos;
using Core;
using Core.Api.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("admin/user/{id:guid}/chat")]
public class AdminController(ICurrentUser currentUser, IHttpContextAccessor httpContextAccessor): ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public PaginatedResult<Chat> GetAllChats(Guid user, GetAllOptionsQuery options)
    {
        return new PaginatedResult<Chat>() { Data = [], Meta = new PaginatedResultMeta() };
    }
    
    [HttpGet("{chatId:guid}")]
    [Authorize(Roles = "Admin")]
    public PaginatedResult<Message> GetAllMessages(Guid user, Guid chatId, GetAllOptionsQuery options)
    {
        return new PaginatedResult<Message>() { Data = [], Meta = new PaginatedResultMeta() };
    }
    
    private Guid GetUserIdOrThrow() => currentUser.GetUserGuidOrThrow(httpContextAccessor.HttpContext?.User.Claims);
}