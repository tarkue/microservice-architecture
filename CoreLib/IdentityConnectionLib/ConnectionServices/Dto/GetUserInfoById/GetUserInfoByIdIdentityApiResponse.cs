using Core.Entities;

namespace ProfileConnectionLib.ConnectionServices.Dto.GetUserInfoById;

public class GetUserInfoByIdIdentityApiResponse: IUserInfo
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public IResource? Photo { get; init; }
}