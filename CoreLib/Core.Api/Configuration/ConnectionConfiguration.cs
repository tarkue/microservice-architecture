namespace Core.Api.Configuration;

public interface IConnectionConfiguration
{
    string ApiProtocol { get; init; }
    string ApiHost { get; init;  }
}