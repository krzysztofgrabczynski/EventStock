using Microsoft.AspNetCore.Http;

namespace EventStock.Application.Interfaces
{
    public interface IJwtTokentService
    {
        string? GetIdFromToken(string token);
        string GetTokenFromHeader(IHeaderDictionary headers);
        string GenerateJWT(string id, string email);
    }
}
