using EventStock.Application.ResultPattern.Errors;
using EventStock.Application.ResultPattern;
using EventStock.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventStock.Domain.Interfaces;
using System.Security.Cryptography;
using EventStock.Application.Dto.Token;
using EventStock.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace EventStock.Application.Services
{
    public class TokenManagementService : ITokenManagementService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<User> _userManager;
        public TokenManagementService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, UserManager<User> userManager)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }

        private string GenerateJWT(string userId, IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpirationTime"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = credentials,

            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        private async Task<string> GenerateRefreshTokenAsync(string userId)
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

        public async Task<Result<TokensResultDto>> GenerateTokensAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var newRefreshToken = await GenerateRefreshTokenAsync(user.Id);
            var newJwtToken = GenerateJWT(user.Id, userRoles);
            var result = new TokensResultDto(newJwtToken, newRefreshToken);

            return Result<TokensResultDto>.Success(result);
        }

        public async Task<Result<TokensResultDto>> GenerateTokensAsync(RefreshToken refreshToken, User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var newRefreshToken = await UpdateRefreshTokenAsync(refreshToken);
            var newJwtToken = GenerateJWT(user.Id, userRoles);
            var result = new TokensResultDto(newJwtToken, newRefreshToken);

            return Result<TokensResultDto>.Success(result);
        }

        private async Task<string> UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            var newToken = Guid.NewGuid().ToString();
            refreshToken.Token = HashToken(newToken);
            refreshToken.Expiration = DateTime.UtcNow.AddDays(int.Parse(_configuration["RefreshToken:ExpirationTimeInDays"]));
            await _refreshTokenRepository.UpdateRefreshTokenAsync(refreshToken);
            return newToken;
        }

        public async Task<Result<RefreshToken>> GetRefreshTokenAsync(string refreshTokenDto, bool needHash)
        {
            var refreshToken = needHash ? HashToken(refreshTokenDto) : refreshTokenDto;
            var token = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);
            if (token == null)
            {
                return Result<RefreshToken>.Failure(new InvalidRefreshTokenResultError());
            }
            return Result<RefreshToken>.Success(token);
        }

        public async Task DeleteRefreshTokenAsync(string refreshTokenDto, bool needHash)
        {
            var refreshToken = needHash ? HashToken(refreshTokenDto) : refreshTokenDto;
            await _refreshTokenRepository.DeleteRefreshTokenAsync(refreshToken);
        }

        public Result CheckRefreshTokenExpiration(RefreshToken tokenFromDB)
        {
            if (tokenFromDB.Expiration < DateTime.UtcNow)
            {
                return Result.Failure(new RefreshTokenExpiredResultError());
            }

            return Result.Success();
        }

        public async Task RevokeRefreshTokensAsync(string userId)
        {
            await _refreshTokenRepository.RevokeRefreshTokensAsync(userId);
        }
    }
}
