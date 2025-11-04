using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using IPermission = Core.Entities.IPermission;

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

    public ICollection<GroupDal> Groups { get; } = [];
    public ICollection<PermissionGroupDal> PermissionGroups { get; } = [];
}