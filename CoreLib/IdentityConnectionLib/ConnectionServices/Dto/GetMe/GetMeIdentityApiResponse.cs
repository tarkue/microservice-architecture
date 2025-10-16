using Core.Entities;

namespace ProfileConnectionLib.ConnectionServices.Dto.GetMe;

public class GetMeIdentityApiResponse: IUser
{
    public Guid Id { get; init; }
    public required string  Name { get; init; }
    public required string Email { get; init; }
}