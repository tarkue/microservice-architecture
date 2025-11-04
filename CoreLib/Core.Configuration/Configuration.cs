namespace Core.Configuration;

public class EnvConfiguration
{
    protected static string Require(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception($"Missing required environment variable: {name}");
        return value;
    }
}