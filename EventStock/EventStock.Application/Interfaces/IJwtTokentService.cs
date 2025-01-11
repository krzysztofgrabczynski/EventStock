using Microsoft.AspNetCore.Http;

namespace EventStock.Application.Interfaces
{
    public interface IJwtTokentService
    {
        string? GetIdFromJwtToken(IHeaderDictionary headers);
        string GenerateJWT(string userId);
    }
}
