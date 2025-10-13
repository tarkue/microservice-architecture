using Logic.Interfaces;

namespace Logic;

public class EnvConfiguration: IConfiguration
{
    public IAuthConfig Auth { get; } = new AuthConfig();
    public IRedisConfig Redis { get; } = new RedisConfig();
    public IDatabaseConfig Database { get; } = new DatabaseConfig();

    public void ValidateAll()
    {
        _ = Auth.Issuer;
        _ = Auth.Audience;
        _ = Auth.Key;
        _ = Redis.ConnectionString;
        _ = Redis.InstanceName;
        _ = Database.ConnectionString;
    }

    private static string Require(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception($"Missing required environment variable: {name}");
        return value;
    }

    private class AuthConfig: IAuthConfig
    {
        public string Issuer => Require("AUTH_ISSUER");
        public string Audience => Require("AUTH_AUDIENCE");
        public string Key => Require("AUTH_KEY");
    }

    private class RedisConfig: IRedisConfig
    {
        private string Host => Require("REDIS_HOST");
        private string Port => Require("REDIS_PORT");
        private string Username => Require("REDIS_USERNAME");
        private string Password => Require("REDIS_PASSWORD");

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


