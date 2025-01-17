using EventStock.Application.Services;
using EventStock.Domain.Interfaces;
using EventStock.Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;


namespace EventStock.Tests
{
    public class TokenManagementTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TokenManagementService _tokenManagementService;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;

        public TokenManagementTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _tokenManagementService = new TokenManagementService(_configurationMock.Object, _refreshTokenRepositoryMock.Object);
        }

        [Fact]
        public async Task GenerateTokensAsyncTest()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("supersecretkeyforsigningsupersecretkeyforsigning");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
            _configurationMock.Setup(c => c["Jwt:ExpirationTime"]).Returns("60");

            var refreshTokenDto = new RefreshToken()
            {
                UserId = Guid.NewGuid().ToString(),
                Token = "hashed-refresh-token",
                Expiration = DateTime.UtcNow.AddDays(7)
            };
            _configurationMock.Setup(c => c["RefreshToken:ExpirationTimeInDays"]).Returns("7");

            // Act
            var tokens = await _tokenManagementService.GenerateTokensAsync(id);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(tokens.Value.AccessToken);
            var refreshToken = tokens.Value.RefreshToken;

            // Assert
            Assert.NotNull(jwtToken);
            Assert.Equal(id, jwtToken.Subject);
            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Contains("TestAudience", jwtToken.Audiences);

            Assert.NotNull(refreshToken);
            Assert.NotEqual(refreshTokenDto.Token, refreshToken);
        }

        [Fact]
        public async Task GetRefreshTokenAsyncTest()
        {
            // Arrange
            var refreshToken = "test-refresh-token";
            var returned = new RefreshToken()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "test-user-id",
                Token = refreshToken,
                CreationAt = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(7)
            };
            _refreshTokenRepositoryMock.Setup(m => m.GetRefreshTokenAsync(refreshToken)).ReturnsAsync(returned);

            // Act
            var result = await _tokenManagementService.GetRefreshTokenAsync(refreshToken, needHash: false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Value.Token, refreshToken);
        }

        [Fact]
        public void CheckIfRefreshTokenExpiredPositiveTest()
        {
            // Arrange
            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "test-user-id",
                Token = "refresh-token-test",
                CreationAt = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(1)
            };

            // Act
            var result = _tokenManagementService.CheckRefreshTokenExpiration(refreshToken);

            // Assert
            Assert.True(result.Succeeded);
        }

        [Fact]
        public void CheckIfRefreshTokenExpiredNegativeTest()
        {
            // Arrange
            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "test-user-id",
                Token = "refresh-token-test",
                CreationAt = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(-1)
            };

            // Act
            var result = _tokenManagementService.CheckRefreshTokenExpiration(refreshToken);

            // Assert
            Assert.False(result.Succeeded);
        }
    }
}
