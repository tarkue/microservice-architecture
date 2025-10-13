namespace Core.Configuration.Interfaces;

public interface IAuthConfig
{
    string Issuer { get; }
    string Audience { get; }
    string Key { get; }
}

