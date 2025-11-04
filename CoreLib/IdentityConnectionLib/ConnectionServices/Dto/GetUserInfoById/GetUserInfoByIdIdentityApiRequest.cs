using ProfileConnectionLib.ConnectionServices.Dto.Shared;

namespace ProfileConnectionLib.ConnectionServices.Dto.GetUserInfoById;

public class GetUserInfoByIdIdentityApiRequest : AuthorizationHeaders
{
    public Guid UserId { get; set; }
}