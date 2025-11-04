using Core;
using Core.Entities;
using Core.Interfaces;
using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IChatService
{
    public Task<IPaginatedResult<Chat>> GetAll(Guid userId, IGetAllOptions options);
    public Task<Guid> Update(IUpdateChat updateChat);
}