using EventStock.Domain.Models;

namespace EventStock.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(string userId);
        Task<RefreshToken?> GetRefreshTokenAsync(string userId);
        bool CheckRefreshToken(RefreshToken tokneFromDB, string tokenFromRequest);
        Task DeleteRefreshTokenAsync(string userId);
    }
}
