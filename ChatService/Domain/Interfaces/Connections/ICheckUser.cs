namespace Domain.Interfaces;

public interface ICheckUser
{
    public Task ThrowExceptionIfNotExistAsync(Guid userId);
}