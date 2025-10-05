using System.Net;
using Domain.Interfaces;
using IdentityService.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace IdentityService.Controllers;

[ApiController]
[Controller]
public class UserController(IUserService userService)
{
    [HttpGet("admin/users")]
    public async Task<IPaginatedResult<UserResponse>> GetAll(PaginatedWithSearchResponse paginatedWithSearchResponse)
    {
        var paginatedResult = await userService.GetAllAsync(
            paginatedWithSearchResponse.PageIndex,
            paginatedWithSearchResponse.PageSize,
            paginatedWithSearchResponse.Search);
        
        return UserResponse.FromDal(paginatedResult);
    }

    [HttpGet("admin/users/{id:guid}")]
    public async Task<UserResponse> GetById(Guid id)
    {
        var user = await userService.FindByIdAsync(id);
        if (user == null)
        {
            throw new BadHttpRequestException("User not found", (int)HttpStatusCode.NotFound);
        }
        return UserResponse.FromDal(user);
    }
}