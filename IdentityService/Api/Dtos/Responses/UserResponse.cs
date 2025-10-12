using Dal.Models;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.OpenApi.Validations.Rules;

namespace IdentityService.Dtos;

public class UserResponse: IUser
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }

    public required UserRole  Role { get; init; }
    
    public ResourceResponse? Photo { get; init; }

    public static UserResponse FromDal(UserDal user)
    {
        return new UserResponse()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Photo =  user.Photo != null ? ResourceResponse.FromDal(user.Photo) : null,
        };
    }

    public static IPaginatedResult<UserResponse> FromDal(IPaginatedResult<UserDal> paginatedResult)
    {
        return new PaginatedResult<UserResponse>()
        {
            Meta = paginatedResult.Meta,
            Data = paginatedResult.Data.Select(FromDal),
        };
    }
}

