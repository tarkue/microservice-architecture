using Api.Dtos;
using Core;
using Core.Api.Interfaces;
using Core.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/chat")]
public class ChatController(ICurrentUser currentUser, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    /// <summary>
    /// Метод получения всех чатов авторизованного пользователя
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "User")]
    public PaginatedResult<Chat> GetAll(GetAllOptionsQuery options)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Обновление метаинформации чата
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateChat"></param>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "User")]
    public void Update(Guid id, [FromBody] IUpdateChat updateChat)
    {
        throw new NotImplementedException();
    }
    
    private Guid GetUserIdOrThrow() => currentUser.GetUserGuidOrThrow(httpContextAccessor.HttpContext?.User.Claims);
}