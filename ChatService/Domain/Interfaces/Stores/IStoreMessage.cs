using Core.Interfaces;
using Domain.Entities;

namespace Domain.Interfaces.Stores;

public interface IStoreMessage: IRepository<Message, Guid> { }