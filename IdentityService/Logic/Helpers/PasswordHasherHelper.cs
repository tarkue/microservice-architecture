using Dal.Models;
using Microsoft.AspNetCore.Identity;

namespace Logic.Helpers;

public class PasswordHasherHelper
{
    private readonly PasswordHasher<UserDal> _passwordHasher = new();
    
    public UserDal GetUserWithHashedPassword(UserDal user)
    {
        var password = _passwordHasher.HashPassword(user, user.Password);
        
        return new UserDal() 
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = password,
            Role = user.Role
        };
    }

    public bool VerifyUserWithHashedPassword(UserDal user, string password)
    {
        var res = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        return res is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}