using EventStock.Application.Dto.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EventStock.Application.Interfaces
{
    public interface IUserAuthenticationService
    {
        string? GetIdFromToken(string token);
        string GetTokenFromHeader(IHeaderDictionary headers);
        string GenerateJWT(string id, string email);
    }
}
