using Domain.Entities;

namespace IdentityService.Dtos;

public class CreateUserRequest: IUserCreate
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}