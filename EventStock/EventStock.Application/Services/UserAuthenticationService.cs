using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventStock.Application.Services
{
    public sealed class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        public UserAuthenticationService(SignInManager<User> signInManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public Task Login(LoginUserDto user)
        {
            throw new NotImplementedException();
        }

        public async Task<SignInResult> AuthenticateUser(LoginUserDto user)
        {
            return await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);
        }
        
        public string GenerateJWT(LoginUserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
            };
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationTime")),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = credentials,

            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
