using Dal;
using Dal.Models;
using Domain.Entities;
using Domain.Enums;
using Logic.Exceptions;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic;

public class PermissionService(AppDbContext context) : BaseService<PermissionDal, Guid>(context.Permissions, context), IPermissionService
{
    public Task<List<PermissionDal>> FindByUserAsync(Guid userId)
    {
        return context.Permissions
            .Where(p => p.Groups.Any(
                g => g.Users.Any(
                    u => u.Id == userId)))
            .ToListAsync();
    }
    
    public async Task CreateAsync(Guid userId, ICreatePermission createPermission)
    {
        var user = await context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new NotFoundException("User");
        }
        
        Guid? objectId;
        PermissionDal? permissionDal;
        
        if (createPermission.Type == PermissionType.Chat)
        {
            if (createPermission.ChatId == Guid.Empty)
            {
                throw new ArgumentException("ChatId is required if Type is Chat");
            }
            
            permissionDal = new PermissionDal
            {
                Id = Guid.NewGuid(),
                Type = PermissionType.Chat,
                ChatId = createPermission.ChatId
            };
            objectId = createPermission.ChatId;
        }
        else
        {
            if (createPermission.ResourceId == Guid.Empty)
            {
                throw new ArgumentException("ResourceId is required if Type is Resource");
            }
            
            permissionDal = new PermissionDal
            {
                Id = Guid.NewGuid(),
                Type = PermissionType.Resource,
                ResourceId = createPermission.ResourceId
            };
            objectId = createPermission.ResourceId;
        }
        
        var groupDal = await context.Groups.FirstOrDefaultAsync(g => g.Name == objectId.ToString());

        if (groupDal == null)
        {
            groupDal = new GroupDal()
            {
                Id = Guid.NewGuid(),
                Name = objectId.ToString()
            };
            context.Groups.Add(groupDal);
        }

        context.Permissions.Add(permissionDal);
        groupDal.Users.Add(user);
        groupDal.Permissions.Add(permissionDal);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId, Guid permissionId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User");
        }
        
        var permission = await context.Permissions.FindAsync(permissionId);
        if (permission == null)
        {
            throw new NotFoundException("Permission");
        }
        
        var group = await context.Groups.FirstOrDefaultAsync(
            g => g.Permissions.Any(p => p.Id == permission.Id) 
                 && g.Users.Any(u => u.Id == user.Id));

        if (group == null)
        {
            throw new NotFoundException("Group");
        }
        
        group.Permissions.Remove(permission);
        group.Users.Remove(user);

        if (group.Permissions.Count == 0 && group.Users.Count == 0)
        {
            context.Groups.Remove(group);
        }
        await context.SaveChangesAsync();
    }
}