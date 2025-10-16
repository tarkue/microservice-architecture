namespace Core.Api.HttpService;

using Polly;

public class PollyExample
{
    public static async Task<string> ActionAsync()
    {
        var res = await Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(
                i => TimeSpan.FromSeconds(5 + i), (_, retryCount, _) =>
                {
                    Console.WriteLine($"Начало {retryCount} Попытки повтора");
                    return Task.CompletedTask;
                })
            .ExecuteAsync(() => Task.FromResult("hello world"));

        return res;
    }
}