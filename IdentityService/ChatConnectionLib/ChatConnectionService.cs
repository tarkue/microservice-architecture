using ChatConnectionLib.ConnectionServices.Dto.UpdateChatsWithUser;
using ChatConnectionLib.ConnectionServices.Interfaces;
using Core.Api;

namespace ChatConnectionLib;

public class ChatConnectionService: ConnectionService, IChatConnectionService
{
    public async Task UpdateChatsWithUser(UpdateChatsWithUserRequest request)
    {
        var builder = new UriBuilder(Configuration!.ApiHost)
        {
            Path = $"user-with-chat-update"
        };
        
        await Patch(builder.Uri, request);
    }
}