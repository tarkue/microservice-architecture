using Core.Entities;
using Core.Interfaces;
using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IMessageService
{
    public Task<IPaginatedResult<Message>> Get(Guid userId);
    public Task<Guid> Send(Guid chatId, IMessageCreate messageCreate);
}