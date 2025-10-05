using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.Enums;

namespace Dal.Models;

[Table("Permissions")]
public record PermissionDal: BaseDal, IPermission
{
    [Column("type")]
    public PermissionType Type { get; init; }
    
    [Column("chatId")]
    public Guid? ChatId { get; init; }
    
    [Column("resourceId")]
    public Guid? ResourceId { get; init; }

    public List<GroupDal> Groups { get; } = [];
    public List<PermissionGroupDal> PermissionGroups { get; } = [];
}