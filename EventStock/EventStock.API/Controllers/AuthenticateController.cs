using EventStock.Application.Dto.RefreshToken;
using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventStock.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var checkUserAutchentication = AuthenticateUser(loginUserDto);
            if(checkUserAutchentication.Result.Succeeded)
            {
                var userId = await _userService.GetUserIdByEmailAsync(loginUserDto.Email);
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound(new { message = "User with provided email not found" });
                }
                var jwtToken = _jwtTokenService.GenerateJWT(userId);
                var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(userId);
                return Ok(new 
                { 
                    Token = jwtToken,
                    RefreshToken = refreshToken
                });
            }

            return BadRequest(new { message = "Invalid login credentials" });
        }

        private async Task<Microsoft.AspNetCore.Identity.SignInResult> AuthenticateUser(LoginUserDto user)
        {
            return await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto refreshTokenDto)
        {
            await _refreshTokenService.DeleteRefreshTokenAsync(refreshTokenDto.RefreshToken, needHash: true);

            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenDto)
        {
            var userId = _jwtTokenService.GetIdFromJwtToken(HttpContext.Request.Headers);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }

            var refreshTokenFromDB = await _refreshTokenService.GetRefreshTokenAsync(refreshTokenDto.RefreshToken, needHash: true);
            if (refreshTokenFromDB == null || !_refreshTokenService.CheckRefreshToken(refreshTokenFromDB))
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }

            var newRefreshToken = await _refreshTokenService.UpdateRefreshTokenAsync(refreshTokenFromDB);
            var newJwtToken = _jwtTokenService.GenerateJWT(userId);

            return Ok(new
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpDelete("revoke-tokens")]
        public async Task<IActionResult> RevokeRefreshTokens()
        {
            var userId = _jwtTokenService.GetIdFromJwtToken(HttpContext.Request.Headers);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid access token" });
            }

            await _refreshTokenService.RevokeRefreshTokensAsync(userId);

            return Ok();
        }
    }
}
