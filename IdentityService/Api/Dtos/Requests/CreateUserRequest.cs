using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace IdentityService.Dtos.Requests;

public class CreateUserRequest: IUserCreate
{
    [MaxLength(100)]
    [Required]
    public required string Name { get; init; }
    
    [MaxLength(100)]
    [EmailAddress]
    [Required]
    public required string Email { get; init; }
    
    [MaxLength(100)]
    [MinLength(8)]
    [Required]
    public required string Password { get; init; }
}