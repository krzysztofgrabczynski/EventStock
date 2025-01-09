using EventStock.Application.Interfaces;
using EventStock.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EventStock.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            var refreshToken = Guid.NewGuid().ToString();

            var hashedRefreshToken = HashToken(refreshToken);   
            await _refreshTokenRepository.SaveRefreshTokenAsync(userId, refreshToken);
            
            return refreshToken;
        }

        private string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }
    }
}
