using Core.Entities;
using ProfileConnectionLib.ConnectionServices.Dto.GetMe;
using ProfileConnectionLib.ConnectionServices.Dto.GetPermissions;
using ProfileConnectionLib.ConnectionServices.Dto.GetUserInfoById;

namespace ProfileConnectionLib.ConnectionServices.Interfaces;

public interface IIdentityConnectionService
{
    GetMeIdentityApiResponse GetMe(GetMeIdentityApiRequest request);
    GetUserInfoByIdIdentityApiResponse GetUserInfoById(GetUserInfoByIdIdentityApiRequest request);
    GetPermissionsIdentityApiResponse GetPermissions(GetPermissionsIdentityApiRequest request);
}