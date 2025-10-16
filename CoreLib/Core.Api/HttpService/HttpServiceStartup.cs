using Core.Api.HttpService.Services;
using Core.Api.HttpService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Core.Api.HttpService;

/// <summary>
/// Регистрация в DI сервисов для HTTP-соединений
/// </summary>
public static class HttpServiceStartup
{
    /// <summary>
    /// Добавление сервиса для осуществления запросов по HTTP
    /// </summary>
    public static IServiceCollection AddHttpRequestService(this IServiceCollection services)
    {
        services
            .AddSingleton<IHttpRequestService, HttpRequestService>()
            .AddHttpClient()
            .AddTransient<IHttpConnectionService, HttpConnectionService>();
        
        services.TryAddTransient<IHttpRequestService, HttpRequestService>();
        
        return services;
    }
}