using ChatConnectionLib.ConnectionServices.Dto.UpdateChatsWithUser;
using Core.Entities;

namespace Dal.Events;

public class ChatWithUserUpdateRequested : UpdateChatsWithUserRequest
{
    public required UserUpdateData OldData { get; init; }
    public required DateTime Timestamp { get; init; }
}