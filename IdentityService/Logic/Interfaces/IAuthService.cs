using Domain.Entities;
using Logic.Models;

namespace Logic.Interfaces;

public interface IAuthService
{
    public Task RegisterAsync(IUserCreate request);
    public Task<ITokenSet> LoginAsync(string email, string password);
    public Task<ITokenSet> RefreshAsync(string refreshToken);
}