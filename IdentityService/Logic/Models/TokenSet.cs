using Logic.Interfaces;

namespace Logic.Models;

public class TokenSet: ITokenSet
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public int ExpiresIn { get; init; }
}