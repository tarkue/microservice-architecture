using System.Text.RegularExpressions;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<GroupDal> Groups { get; set; }
    public DbSet<UserDal> Users { get; set; }
    public DbSet<PermissionDal> Permissions { get; set; }
    
    public DbSet<GroupUserDal> GroupUsers { get; set; }
    public DbSet<PermissionGroupDal> PermissionGroups { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация для UserDal
        modelBuilder.Entity<UserDal>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(u => u.Name).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        // Конфигурация для GroupDal
        modelBuilder.Entity<GroupDal>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(g => g.Name).IsUnique();
        });

        // Конфигурация для PermissionDal
        modelBuilder.Entity<PermissionDal>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Type).IsRequired();
            entity.Property(p => p.ChatId).HasMaxLength(36);
            entity.Property(p => p.ResourceId).HasMaxLength(36);
        });

        // Configure the join entity for Group and User
        modelBuilder.Entity<GroupUserDal>()
            .HasKey(gu => new { gu.GroupId, gu.UserId });

        modelBuilder.Entity<GroupUserDal>()
            .HasOne(gu => gu.Group)
            .WithMany(g => g.GroupUsers)
            .HasForeignKey(gu => gu.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupUserDal>()
            .HasOne(gu => gu.User)
            .WithMany(u => u.GroupUsers)
            .HasForeignKey(gu => gu.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the join entity for Permission and Group
        modelBuilder.Entity<PermissionGroupDal>()
            .HasKey(pg => new { pg.GroupId, pg.PermissionId });
        
        modelBuilder.Entity<PermissionGroupDal>()
            .HasOne(pg => pg.Group)
            .WithMany(g => g.PermissionGroups)
            .HasForeignKey(pg => pg.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<PermissionGroupDal>()
            .HasOne(pg => pg.Permission)
            .WithMany(p => p.PermissionGroups)
            .HasForeignKey(pg => pg.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}