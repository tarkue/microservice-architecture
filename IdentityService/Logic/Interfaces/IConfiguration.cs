namespace Logic.Interfaces;

public interface IConfiguration
{
    IAuthConfig Auth { get; }
    IRedisConfig Redis { get; }
    IDatabaseConfig Database { get; }

    void ValidateAll();
}


