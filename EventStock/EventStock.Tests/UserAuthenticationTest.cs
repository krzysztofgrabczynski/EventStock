using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Application.Services;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStock.Tests
{
    public class UserAuthenticationTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly UserAuthenticationService _authenticationService;

        public UserAuthenticationTest()
        {
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<User>>(
                    userManagerMock.Object,
                    Mock.Of<IHttpContextAccessor>(),
                    Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                    null,
                    null,
                    null,
                    null
                );

            _configurationMock = new Mock<IConfiguration>();
            _authenticationService = new UserAuthenticationService(_signInManagerMock.Object, _configurationMock.Object);
        }

        [Fact]
        public void GenerateJwtTest()
        {
            // Arrange
            var user = new LoginUserDto { Email = "test@example.com", Password = "SecretPassword!"};
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("supersecretkeyforsigningsupersecretkeyforsigning");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
            _configurationMock.Setup(c => c["Jwt:ExpirationTime"]).Returns("60");

            // Act
            var token = _authenticationService.GenerateJWT(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Assert
            Assert.NotNull(jwtToken);
            Assert.Equal("test@example.com", jwtToken.Subject);
            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Contains("TestAudience", jwtToken.Audiences);
        }
    }
}
