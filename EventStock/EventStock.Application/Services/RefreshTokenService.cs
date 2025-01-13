using EventStock.Application.Interfaces;
using EventStock.Domain.Interfaces;
using EventStock.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace EventStock.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            var refreshToken = Guid.NewGuid().ToString();

            var hashedRefreshToken = HashToken(refreshToken);   
            var expirationTime = DateTime.UtcNow.AddDays(int.Parse(_configuration["RefreshToken:ExpirationTimeInDays"]));
            var refreshTokenToSave = new RefreshToken()
            {
                UserId = userId,
                Token = hashedRefreshToken,
                Expiration = expirationTime,
            };
            await _refreshTokenRepository.SaveRefreshTokenAsync(refreshTokenToSave);
            
            return refreshToken;
        }

        private string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshTokenDto, bool needHash)
        {
            var refreshToken = needHash ? HashToken(refreshTokenDto) : refreshTokenDto;
            var token = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);
            if (token == null)
            {
                return null;
            }
            return token;
        }

        public async Task DeleteRefreshTokenAsync(string refreshTokenDto, bool needHash)
        {
            var refreshToken = needHash ? HashToken(refreshTokenDto) : refreshTokenDto;
            await _refreshTokenRepository.DeleteRefreshTokenAsync(refreshToken);
        }

        public bool CheckRefreshToken(RefreshToken tokenFromDB)
        {
            if (tokenFromDB.Expiration < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        public async Task RevokeRefreshTokensAsync(string userId)
        {
            await _refreshTokenRepository.RevokeRefreshTokensAsync(userId);
        }

        public async Task<string> UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            var newToken = Guid.NewGuid().ToString();
            refreshToken.Token = HashToken(newToken);
            refreshToken.Expiration = DateTime.UtcNow.AddDays(int.Parse(_configuration["RefreshToken:ExpirationTimeInDays"]));
            await _refreshTokenRepository.UpdateRefreshTokenAsync(refreshToken);
            return newToken;
        }
    }
}
