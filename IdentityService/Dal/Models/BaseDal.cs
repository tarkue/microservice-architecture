using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

public record BaseDal
{
    [Column("id")]
    [Key]
    public Guid Id { get; init; }
}