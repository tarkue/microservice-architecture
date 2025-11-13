using Api.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class SagaController
{
    [Authorize(Roles = "User")]
    [HttpPatch("user-with-chat-update")]
    public Task UpdateUserWithChat([FromBody] UpdateUserWithChatRequest updateUserWithChatRequest)
    {
        throw new NotImplementedException();
    }
}