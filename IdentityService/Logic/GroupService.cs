using Dal;
using Dal.Models;
using Domain.Entities;
using Domain.Interfaces;
using Logic.Extensions;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic;

public class GroupService(AppDbContext context) : BaseService<GroupDal, Guid>(context.Groups, context), IGroupService
{
    public new Task<IPaginatedResult<GroupDal>> GetAllAsync(int pageIndex, int pageSize)
    {
        return context.Groups
            .OrderBy(g => g.Name)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToPaginatedResultAsync(pageIndex, pageSize);
    }

    public async Task<List<GroupDal>> FindByUserAsync(Guid userId)
    {
        return await context.Groups
            .Where(
                gr => gr.GroupUsers.Any(u => u.UserId == userId))
            .ToListAsync();
    }
}