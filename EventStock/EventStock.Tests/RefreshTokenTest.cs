using EventStock.Application.Interfaces;
using EventStock.Application.Services;
using EventStock.Domain.Interfaces;
using EventStock.Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace EventStock.Tests
{
    public class RefreshTokenTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
        //private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["RefreshToken:ExpirationTimeInDays"]).Returns("7");
            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            //_refreshTokenService = new RefreshTokenService(_configurationMock.Object, _refreshTokenRepository.Object);
        }

        //[Fact]
        //public async Task GetRefreshTokenAsyncTest()
        //{
        //    // Arrange
        //    var refreshToken = "test-refresh-token";
        //    var returned = new RefreshToken()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        UserId = "test-user-id",
        //        Token = refreshToken,
        //        CreationAt = DateTime.UtcNow,
        //        Expiration = DateTime.UtcNow.AddDays(7)
        //    };
        //    _refreshTokenRepository.Setup(m => m.GetRefreshTokenAsync(refreshToken)).ReturnsAsync(returned);
            
        //    // Act
        //    var result = await _refreshTokenService.GetRefreshTokenAsync(refreshToken, needHash: false);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(result.Token, refreshToken);
        //}

        //[Fact]
        //public void CheckRefreshTokenValidTest()
        //{
        //    // Arrange
        //    var refreshToken = new RefreshToken()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        UserId = "test-user-id",
        //        Token = "refresh-token-test",
        //        CreationAt = DateTime.UtcNow,
        //        Expiration = DateTime.UtcNow.AddDays(1)
        //    };

        //    // Act
        //    var result = _refreshTokenService.CheckRefreshToken(refreshToken);

        //    // Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public void CheckRefreshTokenExpiredTest()
        //{
        //    // Arrange
        //    var refreshToken = new RefreshToken()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        UserId = "test-user-id",
        //        Token = "refresh-token-test",
        //        CreationAt = DateTime.UtcNow,
        //        Expiration = DateTime.UtcNow.AddDays(-1)
        //    };

        //    // Act
        //    var result = _refreshTokenService.CheckRefreshToken(refreshToken);

        //    // Assert
        //    Assert.False(result);
        //}

        //[Fact]
        //public async Task UpdateRefreshTokenAsyncTest()
        //{
        //    // Arrange
        //    var tokenBeforeUpdate = "refresh-token-test";
        //    var refreshToken = new RefreshToken()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        UserId = "test-user-id",
        //        Token = tokenBeforeUpdate,
        //        CreationAt = DateTime.UtcNow,
        //        Expiration = DateTime.UtcNow.AddDays(7)
        //    };

        //    // Act
        //    var tokenAfterUpdate = await _refreshTokenService.UpdateRefreshTokenAsync(refreshToken);

        //    // Assert
        //    Assert.NotNull(tokenAfterUpdate);
        //    Assert.NotEqual(tokenBeforeUpdate, tokenAfterUpdate);

        //}
    }
}
