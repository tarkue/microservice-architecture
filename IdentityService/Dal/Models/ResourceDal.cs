using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Core.Enums;

namespace Dal.Models;

[Table("Resources")]
public record ResourceDal: BaseDal, IResource
{
    [MaxLength(500)]
    [Column("source")]
    public required string Source { get; init; }
    
    [Column("type")]
    public ResourceType Type { get; init; }
}