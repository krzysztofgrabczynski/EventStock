using EventStock.Application.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace EventStock.Tests
{
    public class UserAuthenticationTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UserAuthenticationService _authenticationService;

        public UserAuthenticationTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _authenticationService = new UserAuthenticationService(_configurationMock.Object);
        }

        [Fact]
        public void GenerateJwtTest()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            string email = "test@example.com";
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("supersecretkeyforsigningsupersecretkeyforsigning");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
            _configurationMock.Setup(c => c["Jwt:ExpirationTime"]).Returns("60");

            // Act
            var token = _authenticationService.GenerateJWT(id, email);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Assert
            Assert.NotNull(jwtToken);
            Assert.Equal(id, jwtToken.Subject);
            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Contains("TestAudience", jwtToken.Audiences);
        }
    }
}
