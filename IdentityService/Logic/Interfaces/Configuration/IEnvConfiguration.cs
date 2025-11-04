using Core.Configuration.Interfaces;

namespace Logic.Interfaces.Configuration;

public interface IEnvConfiguration: IConfigurationWithAuth
{
    IRedisConfig Redis { get; }
    IDatabaseConfig Database { get; }
}


