using Dal;
using Dal.Models;
using Domain.Interfaces;
using Logic.Extensions;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic;

public class UserService(AppDbContext context): BaseService<UserDal, Guid>(context.Users, context), IUserService
{
    public async Task<IPaginatedResult<UserDal>> GetAllAsync(int pageIndex, int pageSize, string? search)
    {
        return await context.Users
            .Where(u => search == null || u.Name.Contains((search)))
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToPaginatedResultAsync(pageIndex, pageSize);
    }

    public new async Task CreateAsync(UserDal entity)
    {
        context.Users.Add(entity);
        context.Groups.Add(
            new GroupDal()
            {
                Name = entity.Id.ToString()
            });
        await context.SaveChangesAsync();
    }

    public async Task<UserDal?> FindByEmailAsync(string email)
    {
        return await context.Users.FirstAsync(u => u.Email == email);
    }
}