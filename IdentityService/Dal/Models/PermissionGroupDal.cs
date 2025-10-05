using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Dal.Models;

[Table("PermissionGroups")]

public record PermissionGroupDal
{
    public Guid PermissionId { get; init; }
    public required PermissionDal Permission { get; init; }

    public Guid GroupId { get; init; }
    public required GroupDal Group { get; init; }
}