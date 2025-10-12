namespace Logic.Interfaces;

public interface IRedisConfig
{
    string ConnectionString { get; }
    string InstanceName { get; }
}


