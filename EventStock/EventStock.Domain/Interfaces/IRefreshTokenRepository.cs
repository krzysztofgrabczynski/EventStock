using EventStock.Domain.Models;

namespace EventStock.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string userId);
        Task DeleteRefreshTokenAsync(string userId);
    }
}
