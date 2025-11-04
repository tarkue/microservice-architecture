using Core.Entities;

namespace Domain.Interfaces.Connections;

public interface ICheckStorage
{
    public Task ThrowExceptionIfNotExistAsync(IResource resource);
}