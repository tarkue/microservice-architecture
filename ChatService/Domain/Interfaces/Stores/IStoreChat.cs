using Core.Interfaces;
using Domain.Entities;

namespace Domain.Interfaces.Stores;

public interface IStoreChat: IRepository<Chat, Guid> { }