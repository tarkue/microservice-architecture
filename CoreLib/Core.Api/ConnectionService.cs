
using Core.Api.Configuration;
using Core.Api.Dto;
using Core.Api.Enums;
using Core.Api.HttpService.Services;
using Core.Api.HttpService.Services.Interfaces;

namespace Core.Api;

public class ConnectionService
{
    protected IHttpRequestService? Client;
    protected IConnectionConfiguration? Configuration;
    
    protected async Task<TResponse> Get<TRequest, TResponse>(Uri uri, TRequest request) where TRequest : AuthorizationHeaders
    {
        ThrowIfUnsupportedProtocol();
        return await BuildRequest<TRequest, TResponse>(HttpMethod.Get, uri, request);
    }

    protected async Task Patch<TRequest>(Uri uri, TRequest request) 
        where TRequest : AuthorizationHeaders
    {
        ThrowIfUnsupportedProtocol();
        await BuildRequest<TRequest, object>(HttpMethod.Patch, uri, request);
    }

    private async Task<TResponse> BuildRequest<TRequest, TResponse>(HttpMethod method, Uri uri, TRequest request)
        where TRequest: AuthorizationHeaders
    {
        ThrowIfUnsupportedProtocol();
        
        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {request.AccessToken}" }
        };

        request.AccessToken = null;
        
        var response =  Client?.SendRequestAsync<TResponse>(new HttpRequestData()
        {
            ContentType = ContentType.ApplicationJson,
            Method = method,
            Body = request,
            Uri = uri,
            HeaderDictionary = headers,
        })!;
        await response.WaitAsync(CancellationToken.None);
        return response.Result.Body;
    }

    private void ThrowIfUnsupportedProtocol()
    {
        var protocol = GetProtocolByConfiguration();
        if  (protocol != Protocol.Http)
        {
            throw new Exception("Unsupported protocol");
        }
    }

    protected Protocol GetProtocolByConfiguration()
    {
        return Configuration?.ApiProtocol.ToLower() switch
        {
            "grpc" => Protocol.GRpc,
            _ => Protocol.Http
        };
    }
}