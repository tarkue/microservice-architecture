namespace Domain.Interfaces.Connections;

public interface ICheckUser
{
    public Task ThrowExceptionIfNotExistAsync(Guid userId);
}