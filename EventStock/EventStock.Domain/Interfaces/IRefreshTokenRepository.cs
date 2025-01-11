using EventStock.Domain.Models;

namespace EventStock.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshTokenDto);
        Task DeleteRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokensAsync(string userId);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
