namespace Api.Dtos;

public class UpdateUserWithChatRequest
{
    public Guid UserId;
    public string? Name;
    public Guid? Photo;
}