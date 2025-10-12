
using IdentityService.Dtos.Requests;
using IdentityService.Dtos.Responses;
using Logic.Exceptions;
using Logic.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IHttpContextAccessor httpContextAccessor, IAuthService authService)
{
    [HttpPost("login")]
    public async Task<ITokenSet> Login([FromBody] LoginRequest request)
    {
        try
        {
            return await authService.LoginAsync(request.Username, request.Password);
        }
        catch (UnauthorizedAccessException)
        {
            throw new BadHttpRequestException("Invalid username or password", StatusCodes.Status401Unauthorized);
        }
    }

    [HttpPost("register")]
    public async Task Register([FromBody] CreateUserRequest createUserRequest)
    {
        await authService.RegisterAsync(createUserRequest);
    }
    
    [HttpPost("refresh-token")]
    public async Task<ITokenSet> RefreshToken([FromBody] TokenResponse tokenResponse)
    {
        var refreshToken = GetRefreshTokenOrThrow(tokenResponse.Token);

        try
        {
            return await authService.RefreshAsync(refreshToken);
        }
        catch (RefreshTokenExpiredException error)
        {
            throw new BadHttpRequestException(error.Message, StatusCodes.Status422UnprocessableEntity);
        }
    }

    private string GetRefreshTokenOrThrow(string? fromBodyToken)
    {
        if (fromBodyToken != null)
            return fromBodyToken;

        if (httpContextAccessor.HttpContext != null)
        {
            httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("refresh_token", out var token);

            if (token != null)
                return token;
        }

        throw new BadHttpRequestException("Invalid refresh token", StatusCodes.Status422UnprocessableEntity);
    }
}