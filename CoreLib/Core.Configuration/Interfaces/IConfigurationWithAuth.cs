namespace Core.Configuration.Interfaces;

public interface IConfigurationWithAuth
{
    public IAuthConfig Auth { get; }
}