using Core.Entities;
using Dal.Models;

namespace IdentityService.Dtos.Responses;

public class UserInfoResponse: IUserInfo
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public IResource? Photo { get; init; }
    
    public static UserInfoResponse FromDal(UserDal user)
    {
        return new UserInfoResponse()
        {
            Id = user.Id,
            Name = user.Name,
            Photo =  user.Photo != null ? ResourceResponse.FromDal(user.Photo) : null,
        };
    }
}