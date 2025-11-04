using Core.Api.HttpService.Services;
using Core.Api.HttpService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ProfileConnectionLib.ConnectionServices.Configuration;
using ProfileConnectionLib.ConnectionServices.Dto.GetMe;
using ProfileConnectionLib.ConnectionServices.Dto.GetPermissions;
using ProfileConnectionLib.ConnectionServices.Dto.GetUserInfoById;
using ProfileConnectionLib.ConnectionServices.Dto.Shared;
using ProfileConnectionLib.ConnectionServices.Enums;
using ProfileConnectionLib.ConnectionServices.Interfaces;

namespace ProfileConnectionLib;

public class IdentityConnectionService(): IIdentityConnectionService
{
    public required IIdentityConnectionConfiguration Configuration;
    private readonly IHttpRequestService? _client;
    
    public IdentityConnectionService(IIdentityConnectionConfiguration configuration, IServiceProvider serviceProvider) :
        this()
    {
        Configuration = configuration;
        var protocol = GetProtocolByConfiguration();

        if (protocol == Protocol.Http)
            _client = serviceProvider.GetRequiredService<IHttpRequestService>();
    }

    public GetMeIdentityApiResponse GetMe(GetMeIdentityApiRequest request)
    {
        var builder = new UriBuilder(Configuration.IdentityApiHost)
        {
            Path = $"user"
        };
        return Get<GetMeIdentityApiResponse, GetMeIdentityApiRequest>(builder.Uri, request);
    }

    public GetUserInfoByIdIdentityApiResponse GetUserInfoById(GetUserInfoByIdIdentityApiRequest request)
    {
        var builder = new UriBuilder(Configuration.IdentityApiHost)
        {
            Path = $"user/{request.UserId}"
        };
        return Get<GetUserInfoByIdIdentityApiResponse, GetUserInfoByIdIdentityApiRequest>(builder.Uri, request);
    }

    public GetPermissionsIdentityApiResponse GetPermissions(GetPermissionsIdentityApiRequest request)
    {
        var builder = new UriBuilder(Configuration.IdentityApiHost)
        {
            Path = $"user/permission"
        };
        return Get<GetPermissionsIdentityApiResponse, GetPermissionsIdentityApiRequest>(builder.Uri, request);
    }

    private TResponse Get<TResponse, TRequest>(Uri uri, TRequest request)
        where TRequest: AuthorizationHeaders
    {
        var protocol = GetProtocolByConfiguration();
        if (protocol != Protocol.Http)
        {
            throw new Exception("Unsupported protocol");
        }
        
        var task = BuildRequest<TResponse, TRequest>(HttpMethod.Get, uri, request);
        task.RunSynchronously();
        return task.Result;
    }

    private async Task<TResponse> BuildRequest<TResponse, TRequest>(HttpMethod method, Uri uri, TRequest request)
        where TRequest: AuthorizationHeaders
    {
        var protocol = GetProtocolByConfiguration();

        if (protocol != Protocol.Http)
        {
            throw new NotImplementedException("Unsupported protocol");
        }
        
        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {request.AccessToken}" }
        };
        var response =  _client?.SendRequestAsync<TResponse>(new HttpRequestData()
        {
            ContentType = ContentType.ApplicationJson,
            Method = HttpMethod.Get,
            Uri = uri,
            HeaderDictionary = headers,
        })!;
        await response.WaitAsync(CancellationToken.None);
        return response.Result.Body;

    }
    
    private Protocol GetProtocolByConfiguration()
    {
        return Configuration.IdentityApiProtocol.ToLower() switch
        {
            "grpc" => Protocol.GRpc,
            _ => Protocol.Http
        };
    }
}