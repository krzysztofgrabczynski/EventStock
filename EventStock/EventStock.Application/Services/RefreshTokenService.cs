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

        public async Task<RefreshToken?> GetRefreshTokenAsync(string userId)
        {
            var token = await _refreshTokenRepository.GetRefreshTokenAsync(userId);
            if (token == null)
            {
                return null;
            }
            return token;
        }

        public async Task DeleteRefreshTokenAsync(string userId)
        {
            await _refreshTokenRepository.DeleteRefreshTokenAsync(userId);
        }

        public bool CheckRefreshToken(RefreshToken tokenFromDB, string tokenFromRequest)
        {
            var hashedTokenFromRequest = HashToken(tokenFromRequest);
            var hashedTokenFromDB = tokenFromDB.Token;

            if (hashedTokenFromDB != hashedTokenFromRequest || tokenFromDB.Expiration < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}
