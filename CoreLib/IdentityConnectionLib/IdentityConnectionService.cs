using Core.Api;
using Core.Api.Configuration;
using Core.Api.Enums;
using Core.Api.HttpService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using IdentityConnectionLib.ConnectionServices.Dto.GetMe;
using IdentityConnectionLib.ConnectionServices.Dto.GetPermissions;
using IdentityConnectionLib.ConnectionServices.Dto.GetUserInfoById;
using IdentityConnectionLib.ConnectionServices.Interfaces;

namespace IdentityConnectionLib;

public class IdentityConnectionService: ConnectionService, IIdentityConnectionService
{
    public IdentityConnectionService(IConnectionConfiguration configuration, IServiceProvider serviceProvider)
    {
        Configuration = configuration;
        var protocol = GetProtocolByConfiguration();

        if (protocol == Protocol.Http)
            Client = serviceProvider.GetRequiredService<IHttpRequestService>();
    }

    public Task<GetMeIdentityApiResponse> GetMe(GetMeIdentityApiRequest request)
    {
        var builder = new UriBuilder(Configuration!.ApiHost)
        {
            Path = $"user"
        };
        return Get<GetMeIdentityApiRequest, GetMeIdentityApiResponse>(builder.Uri, request);
    }

    public Task<GetUserInfoByIdIdentityApiResponse> GetUserInfoById(GetUserInfoByIdIdentityApiRequest request)
    {
        var builder = new UriBuilder(Configuration!.ApiHost)
        {
            Path = $"user/{request.UserId}"
        };
        return Get<GetUserInfoByIdIdentityApiRequest, GetUserInfoByIdIdentityApiResponse>(builder.Uri, request);
    }

    public Task<GetPermissionsIdentityApiResponse> GetPermissions(GetPermissionsIdentityApiRequest request)
    {
        var builder = new UriBuilder(Configuration!.ApiHost)
        {
            Path = $"user/permission"
        };
        return Get<GetPermissionsIdentityApiRequest, GetPermissionsIdentityApiResponse>(builder.Uri, request);
    }
}