namespace IdentityService.Dtos.Responses;

public class UserUpdateResponse
{
    public Guid UserId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}