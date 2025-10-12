using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dal.Models;
using Domain.Entities;
using Domain.Enums;
using Logic.Exceptions;
using Logic.Helpers;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace Logic;

public class AuthService(IUserService userService, IDistributedCache cache, IConfiguration configuration): IAuthService
{
    private readonly PasswordHasherHelper _passwordHasherHelper = new();
    private const string RefreshPrefix = "refresh:";

    public async Task RegisterAsync(IUserCreate request)
    {
        var user = _passwordHasherHelper.GetUserWithHashedPassword(new UserDal
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            Role = UserRole.User
        });
        
        await userService.CreateFromDalAsync(user);
    }

    public async Task<ITokenSet> LoginAsync(string email, string password)
    {
        var user = await userService.FindByEmailAsync(email) ?? throw new UnauthorizedAccessException("Invalid credentials");
        
        if (!_passwordHasherHelper.VerifyUserWithHashedPassword(user, password))
            throw new UnauthorizedAccessException("Invalid credentials");

        return await IssueTokensAsync(user.Id, user.Name, user.Email, user.Role.ToString());
    }

    public async Task<ITokenSet> RefreshAsync(string refreshToken)
    {
        var userIdString = await cache.GetStringAsync(RefreshPrefix + refreshToken);
        if (userIdString is null)
            throw new RefreshTokenExpiredException();

        if (!Guid.TryParse(userIdString, out var userId))
            throw new RefreshTokenExpiredException();

        var user = await userService.GetByIdOrThrow(userId);
        return await IssueTokensAsync(user.Id, user.Name, user.Email, user.Role.ToString());
    }
    private async Task<ITokenSet> IssueTokensAsync(Guid userId, string name, string email, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, name),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role),
            new(ClaimTypes.Sid, userId.ToString())
        };

        configuration.ValidateAll();
        var issuer = configuration.Auth.Issuer;
        var audience = configuration.Auth.Audience;
        var key = configuration.Auth.Key;

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        var refreshToken = Guid.NewGuid().ToString("N");
        await cache.SetStringAsync(RefreshPrefix + refreshToken, userId.ToString(), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
        });

        return new TokenSet
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = (int)TimeSpan.FromMinutes(30).TotalSeconds
        };
    }
}