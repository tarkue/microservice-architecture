using ChatConnectionLib.ConnectionServices.Dto.UpdateChatsWithUser;
using ChatConnectionLib.ConnectionServices.Interfaces;
using Polly;

namespace Logic.Helpers;

public class UpdateUserPolly(IChatConnectionService chatConnectionService)
{
    public async Task UpdateUser(UpdateChatsWithUserRequest request)
    {
        await Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(
                i => TimeSpan.FromSeconds(5 + i), 
                async (_, retryCount, _) => await chatConnectionService.UpdateChatsWithUser(request))
            .ExecuteAsync(async () => await chatConnectionService.UpdateChatsWithUser(request));
    }
}