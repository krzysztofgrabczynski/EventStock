using EventStock.Application.Dto.User;

namespace EventStock.Application.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateJWT(LoginUserDto user);
        bool AuthenticateUser(LoginUserDto user);
        Task Login(LoginUserDto user);
    }
}
