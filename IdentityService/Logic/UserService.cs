using System.Net;
using Dal;
using Dal.Models;
using Domain.Entities;
using Domain.Interfaces;
using Logic.Extensions;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic;

public class UserService(AppDbContext context): BaseService<UserDal, Guid>(context.Users, context), IUserService
{
    public async Task<IPaginatedResult<UserDal>> GetAllAsync(int? pageIndex, int? pageSize, string? search = null)
    {
        var index = pageIndex ?? 1;
        var size = pageSize ?? 10;
        
        return await context.Users
            .Where(u => search == null || u.Name.Contains((search)))
            .Skip((index - 1) * size)
            .Take(size)
            .ToPaginatedResultAsync(index, size);
    }

    public async Task<UserDal> GetByIdOrThrow(Guid id)
    {
        var user = await FindByIdAsync(id);
        return user ?? throw new ArgumentException("User not found");
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
    
    public async Task UpdateAsync(UserDal userDal, IUserUpdate updateBody)
    {
        await UpdateAsync(new UserDal()
        {
            Id = userDal.Id,
            Email = updateBody.Email ?? userDal.Email,
            Name = updateBody.Name ?? userDal.Name,
            PhotoResourceId = updateBody.Photo ?? userDal.PhotoResourceId,
            Password = userDal.Password,
            Role = userDal.Role,
        });
    }
}