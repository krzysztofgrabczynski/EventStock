using EventStock.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace EventStock.Tests
{
    public class TokenManagementTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        //private readonly JwtTokenService _jwtTokenService;

        public TokenManagementTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            //_jwtTokenService = new JwtTokenService(_configurationMock.Object);
        }

        //[Fact]
        //public void GenerateJwtTest()
        //{
        //    // Arrange
        //    string id = Guid.NewGuid().ToString();
        //    _configurationMock.Setup(c => c["Jwt:Key"]).Returns("supersecretkeyforsigningsupersecretkeyforsigning");
        //    _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        //    _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
        //    _configurationMock.Setup(c => c["Jwt:ExpirationTime"]).Returns("60");

        //    // Act
        //    var token = _jwtTokenService.GenerateJWT(id);
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var jwtToken = tokenHandler.ReadJwtToken(token);

        //    // Assert
        //    Assert.NotNull(jwtToken);
        //    Assert.Equal(id, jwtToken.Subject);
        //    Assert.Equal("TestIssuer", jwtToken.Issuer);
        //    Assert.Contains("TestAudience", jwtToken.Audiences);
        //}

        //[Fact]
        //public void GetIdFromJwtTokenTest()
        //{
        //    // Arrange
        //    var jwtToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIwYmM5YjBkMC1lMmFhLTQ4ZjYtOWFkYy04NTQwZTY4NDEzOGMiLCJzdWIiOiJ1c2VyLWlkIiwibmJmIjoxNzM2NzcxOTMwLCJleHAiOjE3MzY3NzU1MzAsImlhdCI6MTczNjc3MTkzMCwiaXNzIjoidGVzdC1pc3N1ZXIiLCJhdWQiOiJ0ZXN0LWF1ZGllbmNlIn0.CcsE5L31ukNgvp7pxlxaLG0UwQGrFyW1Y-Xh2Jn3qEk";
        //    var sub = "user-id";
        //    var mock = new Mock<IHeaderDictionary>();
        //    mock.Setup(m => m["Authorization"]).Returns(jwtToken);

        //    // Act
        //    var idFromToken = _jwtTokenService.GetIdFromJwtToken(mock.Object);

        //    // Assert
        //    Assert.NotNull(idFromToken);
        //    Assert.Equal(idFromToken, sub);
        //}     
    }
}
