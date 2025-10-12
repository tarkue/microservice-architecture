namespace Logic.Interfaces;

public interface ITokenSet
{
    public string  AccessToken { get; init; }
    public string  RefreshToken { get; init; }
    public int ExpiresIn { get; init; }
}