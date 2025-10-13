using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Models;

[Table("GroupUsers")]
public record GroupUserDal
{
    public Guid GroupId { get; init; }
    public required GroupDal Group { get; init; }
    
    public Guid UserId { get; init; }
    public required UserDal User { get; init; }
}