using EventStock.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventStock.Application.Services
{
    public sealed class JwtTokenService : IJwtTokentService
    {
        private readonly IConfiguration _configuration;
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJWT(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };
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

        public string GetIdFromJwtToken(IHeaderDictionary headers)
        {
            var tokenFromHeader = headers["Authorization"].ToString();
            var token = tokenFromHeader.Substring("Bearer ".Length);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var result = jwtToken.Subject;
            if (result == null)
            {
                throw new SecurityTokenValidationException("Invalid token");
            }
            return result;
        }   
    }
}
