using EventStock.Application.Dto.RefreshToken;
using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventStock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokentService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserService _userService;
        public AuthenticateController(SignInManager<User> signInManager, IJwtTokentService jwtTokenService, IRefreshTokenService refreshTokenService, IUserService userService)
        {
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var checkUserAutchentication = AuthenticateUser(loginUserDto);
            if(checkUserAutchentication.Result.Succeeded)
            {
                var userId = await _userService.GetUserIdByEmailAsync(loginUserDto.Email);
                var jwtToken = _jwtTokenService.GenerateJWT(userId);
                var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(userId);
                return Ok(new 
                { 
                    Token = jwtToken,
                    RefreshToken = refreshToken
                });
            }

            return BadRequest("Invalid login credentials");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = _jwtTokenService.GetIdFromJwtToken(HttpContext.Request.Headers);
            await _refreshTokenService.DeleteRefreshTokenAsync(userId);

            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenDto)
        {
            var userId = _jwtTokenService.GetIdFromJwtToken(HttpContext.Request.Headers);

            var refreshTokenFromDB = await _refreshTokenService.GetRefreshTokenAsync(userId);
            if (!_refreshTokenService.CheckRefreshToken(refreshTokenFromDB, refreshTokenDto.RefreshToken))
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }

            await _refreshTokenService.DeleteRefreshTokenAsync(userId);
            var newJwtToken = _jwtTokenService.GenerateJWT(userId);
            var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(userId);

            return Ok(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        private async Task<Microsoft.AspNetCore.Identity.SignInResult> AuthenticateUser(LoginUserDto user)
        {
            return await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);
        }
    }
}
