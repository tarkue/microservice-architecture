
using IdentityConnectionLib.ConnectionServices.Dto.GetMe;
using IdentityConnectionLib.ConnectionServices.Dto.GetPermissions;
using IdentityConnectionLib.ConnectionServices.Dto.GetUserInfoById;

namespace IdentityConnectionLib.ConnectionServices.Interfaces;

public interface IIdentityConnectionService
{
    Task<GetMeIdentityApiResponse> GetMe(GetMeIdentityApiRequest request);
    Task<GetUserInfoByIdIdentityApiResponse> GetUserInfoById(GetUserInfoByIdIdentityApiRequest request);
    Task<GetPermissionsIdentityApiResponse> GetPermissions(GetPermissionsIdentityApiRequest request);
}