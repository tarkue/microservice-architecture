using Core.Configuration.Interfaces;

namespace Core.Configuration;

public class EnvConfigurationWithAuth: EnvConfiguration, IConfigurationWithAuth
{
    public IAuthConfig Auth { get; } = new AuthConfig();
    
    private class AuthConfig: IAuthConfig
    {
        public string Issuer => Require("AUTH_ISSUER");
        public string Audience => Require("AUTH_AUDIENCE");
        public string Key => Require("AUTH_KEY");
    }
}