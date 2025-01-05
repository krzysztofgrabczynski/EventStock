using EventStock.Application.Dto.User;
using Microsoft.AspNetCore.Identity;

namespace EventStock.Application.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<SignInResult> AuthenticateUser(LoginUserDto user);
        string GenerateJWT(LoginUserDto user);
    }
}
