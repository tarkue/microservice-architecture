using ChatConnectionLib.ConnectionServices.Dto.UpdateChatsWithUser;

namespace ChatConnectionLib.ConnectionServices.Interfaces;

public interface IChatConnectionService
{
    public Task UpdateChatsWithUser(UpdateChatsWithUserRequest request);
}