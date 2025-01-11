using EventStock.Domain.Models;

namespace EventStock.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(string userId);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, bool needHash);
        bool CheckRefreshToken(RefreshToken tokneFromDB, string tokenFromRequest);
        Task DeleteRefreshTokenAsync(string refreshToken, bool needHash);
        Task RevokeRefreshTokensAsync(string userId);
        Task <string> UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
