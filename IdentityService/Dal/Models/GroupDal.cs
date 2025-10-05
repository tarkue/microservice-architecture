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
    
    public List<UserDal> Users { get; } = [];
    public List<PermissionDal> Permissions { get; } = [];
    public List<GroupUserDal> GroupUsers { get; } = [];
    public List<PermissionGroupDal> PermissionGroups { get; } = [];
}