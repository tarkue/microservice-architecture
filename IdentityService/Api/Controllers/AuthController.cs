
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
        var response = httpContextAccessor.HttpContext?.Response;

        if (response == null)
        {
            throw new BadHttpRequestException("Invalid response", StatusCodes.Status400BadRequest);
        }

        try
        {
            var tokenSet = await authService.LoginAsync(request.Username, request.Password);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(tokenSet.ExpiresIn)
            };

            response.Cookies.Append("access_token", tokenSet.AccessToken, cookieOptions);

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
            response.Cookies.Append("refresh_token", tokenSet.RefreshToken, refreshCookieOptions);

            return tokenSet;
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