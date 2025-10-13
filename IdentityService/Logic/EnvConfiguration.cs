using Core.Configuration;
using Logic.Interfaces.Configuration;

namespace Logic;

public class EnvConfiguration: EnvConfigurationWithAuth, IEnvConfiguration
{
    public IRedisConfig Redis { get; } = new RedisConfig();
    public IDatabaseConfig Database { get; } = new DatabaseConfig();

    private class RedisConfig: IRedisConfig
    {
        private static string Host => Require("REDIS_HOST");
        private static string Port => Require("REDIS_PORT");
        private static string Username => Require("REDIS_USERNAME");
        private static string Password => Require("REDIS_PASSWORD");

        public string InstanceName => Require("REDIS_INSTANCE_NAME");
        public string ConnectionString => $"{Host}:{Port},user={Username},password={Password}"; 
    }

    private class DatabaseConfig: IDatabaseConfig
    {
        private static string Host => Require("DATABASE_HOST");
        private static string Port => Require("DATABASE_PORT");
        private static string Name => Require("DATABASE_NAME");
        private static string Username => Require("DATABASE_USERNAME");
        private static string Password => Require("DATABASE_PASSWORD");
        
        public string ConnectionString => $"Host={Host};Port={Port};Database={Name};Username={Username};Password={Password};";
    }
}


