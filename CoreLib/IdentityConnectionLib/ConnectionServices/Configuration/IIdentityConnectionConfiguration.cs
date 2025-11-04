namespace ProfileConnectionLib.ConnectionServices.Configuration;

public interface IIdentityConnectionConfiguration
{
    string IdentityApiProtocol { get; init; }
    string IdentityApiHost { get; init;  }
}