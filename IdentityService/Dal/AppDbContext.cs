using System.Text.RegularExpressions;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<GroupDal> Groups { get; set; }
    public DbSet<UserDal> Users { get; set; }
    public DbSet<PermissionDal> Permissions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the join entity for Group and User
        modelBuilder.Entity<GroupUserDal>()
            .HasKey(gu => new { gu.GroupId, gu.UserId }); // Composite primary key

        // Configure the relationship from GroupUserDal to GroupDal
        modelBuilder.Entity<GroupUserDal>()
            .HasOne(gu => gu.Group)
            .WithMany(g => g.GroupUsers) // You'll need to add this collection to GroupDal
            .HasForeignKey(gu => gu.GroupId);

        // Configure the relationship from GroupUserDal to UserDal
        modelBuilder.Entity<GroupUserDal>()
            .HasOne(gu => gu.User)
            .WithMany(u => u.GroupUsers) // You'll need to add this collection to UserDal
            .HasForeignKey(gu => gu.UserId);

        // Repeat a similar pattern for PermissionGroupDal
        modelBuilder.Entity<PermissionGroupDal>()
            .HasKey(gu => new { gu.GroupId, gu.PermissionId });
        
        modelBuilder.Entity<PermissionGroupDal>()
            .HasOne(gu => gu.Group)
            .WithMany(g => g.PermissionGroups)
            .HasForeignKey(gu => gu.GroupId);
        
        modelBuilder.Entity<PermissionGroupDal>()
            .HasOne(gu => gu.Permission)
            .WithMany(p => p.PermissionGroups)
            .HasForeignKey(gu => gu.PermissionId);
    }
}