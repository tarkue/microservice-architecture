
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
    
    protected Task<TResponse> Get<TRequest, TResponse>(Uri uri, TRequest request) where TRequest : AuthorizationHeaders
    {
        var protocol = GetProtocolByConfiguration();
        if (protocol != Protocol.Http)
        {
            throw new Exception("Unsupported protocol");
        }
        
        return BuildRequest<TRequest, TResponse>(HttpMethod.Get, uri, request);
    }

    private async Task<TResponse> BuildRequest<TRequest, TResponse>(HttpMethod method, Uri uri, TRequest request)
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
        var response =  Client?.SendRequestAsync<TResponse>(new HttpRequestData()
        {
            ContentType = ContentType.ApplicationJson,
            Method = HttpMethod.Get,
            Uri = uri,
            HeaderDictionary = headers,
        })!;
        await response.WaitAsync(CancellationToken.None);
        return response.Result.Body;
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