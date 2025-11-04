using ChatConnectionLib.ConnectionServices.Dto.UpdateChatsWithUser;
using ChatConnectionLib.ConnectionServices.Interfaces;
using Core.Api;

namespace ChatConnectionLib;

public class ChatConnectionService: ConnectionService, IChatConnectionService
{
    public Task UpdateChatsWithUser(UpdateChatsWithUserRequest request)
    {
        throw new NotImplementedException();
    }
}