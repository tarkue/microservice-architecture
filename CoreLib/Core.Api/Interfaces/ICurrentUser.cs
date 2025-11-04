using System.Security.Claims;

namespace Core.Api.Interfaces;

public interface ICurrentUser
{
    public Guid GetUserGuidOrThrow(IEnumerable<Claim>? claims);
}