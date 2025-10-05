using Dal;
using Dal.Models;
using Domain.Interfaces;
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
}