using System.Net;
using System.Security.Claims;
using Core.Api.Interfaces;

namespace Core.Api;

public class CurrentUser : ICurrentUser
{
    public Guid GetUserGuidOrThrow(IEnumerable<Claim>? claims)
    {
        var idInString = claims?.First(c => c.Type.Equals(ClaimTypes.Sid)).Value;
        
        if (claims == null || !Guid.TryParse(idInString, out var id))
            throw new HttpRequestException("User not found",  new ArgumentException(), HttpStatusCode.NotFound);
        
        return id;
    }
}