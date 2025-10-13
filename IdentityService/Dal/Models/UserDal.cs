using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;


[Table("Users")]
[Index(nameof(Email), IsUnique = true)]
public record UserDal: BaseDal, ISecuredUser, IPhoto<ResourceDal>
{
    [MaxLength(100)]
    [Column("name")]
    public required string Name { get; init; } 
    
    [MaxLength(300)]
    [Column("email")]
    public required string Email { get; init; }
    
    [MaxLength(100)]
    [Column("password")]
    public required string Password { get; init; }

    [Column("role")]
    public UserRole Role { get; init; }

    [Column("photoResourceId")] 
    public Guid? PhotoResourceId { get; init; }
    
    [ForeignKey(nameof(PhotoResourceId))]
    public ResourceDal? Photo { get; init; }

    public ICollection<GroupDal> Groups { get; } = [];
    public ICollection<GroupUserDal> GroupUsers { get; } = [];
}