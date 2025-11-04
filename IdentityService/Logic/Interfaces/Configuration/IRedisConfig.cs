namespace Logic.Interfaces.Configuration;

public interface IRedisConfig
{
    string ConnectionString { get; }
    string InstanceName { get; }
}


