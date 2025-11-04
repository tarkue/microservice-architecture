using Core.Api.Dto;

namespace IdentityConnectionLib.ConnectionServices.Dto.GetUserInfoById;

public class GetUserInfoByIdIdentityApiRequest : AuthorizationHeaders
{
    public Guid UserId { get; set; }
}