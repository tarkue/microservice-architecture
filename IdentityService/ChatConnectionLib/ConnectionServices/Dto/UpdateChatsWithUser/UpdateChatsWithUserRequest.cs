using Core.Api.Dto;

namespace ChatConnectionLib.ConnectionServices.Dto.UpdateChatsWithUser;

public class UpdateChatsWithUserRequest: AuthorizationHeaders
{
    public Guid UserId { get; init; }
    public string? Name { get; init; }
    public Guid? Photo { get; init; }
}