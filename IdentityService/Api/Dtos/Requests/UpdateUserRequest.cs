using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace IdentityService.Dtos.Requests;

public class UpdateUserRequest: IUserUpdate
{
    [MaxLength(100)]
    public string? Name { get; init; }
    
    [MaxLength(255)]
    [EmailAddress]
    public string? Email { get; init; }
    public Guid? Photo  { get; init; }
}