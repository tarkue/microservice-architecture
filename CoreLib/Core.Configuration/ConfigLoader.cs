using System.Diagnostics.CodeAnalysis;

namespace Core.Configuration;

public static class EnvConfigLoader
{
    public static void Load<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConfiguration>()
        where TConfiguration : class
    {
        DotNetEnv.Env.Load();
        Activator.CreateInstance<TConfiguration>();
    }
}