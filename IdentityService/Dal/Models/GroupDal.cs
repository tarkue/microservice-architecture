using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

[Table("Groups")]
[Index(nameof(Name), IsUnique = true)]
public record GroupDal :  BaseDal, IGroup
{
    [MaxLength(100)]
    [Column("name")]
    public required string Name { get; set; }
    
    public ICollection<UserDal> Users { get; } = [];
    public ICollection<PermissionDal> Permissions { get; } = [];
    public ICollection<GroupUserDal> GroupUsers { get; } = [];
    public ICollection<PermissionGroupDal> PermissionGroups { get; } = [];
}