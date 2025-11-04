using ProfileConnectionLib.ConnectionServices.Dto.Shared;

namespace ChatConnectionLib.ConenctionServices.Dto.UpdateChatsWithUser;

public class UpdateChatsWithUserRequest: AuthorizationHeaders
{
    public Guid UserId { get; init; }
    public string? Name { get; init; }
    public Guid? Photo { get; init; }
}