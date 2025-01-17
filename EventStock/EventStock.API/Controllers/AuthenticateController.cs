using EventStock.Application.Dto.Token;
using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventStock.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly ITokenManagementService _tokenManagementService;

        private string UserId
        {
            get
            {
                var endpoint = HttpContext.GetEndpoint();
                if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
                {
                    return "";
                }

                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("Invalid access token");
                }
                return userId;
            }
        }

        public AuthenticateController(SignInManager<User> signInManager, IUserService userService, ITokenManagementService tokenManagementService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _tokenManagementService = tokenManagementService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var checkIfUserAutchenticated = await AuthenticateUser(loginUserDto);
            if(!checkIfUserAutchenticated.Succeeded)
            {
                return BadRequest(new { message = "Invalid login credentials" });
            }

            var userId = await _userService.GetUserIdByEmailAsync(loginUserDto.Email);
            if (!userId.Succeeded)
            {
                return BadRequest(userId.Error);
            }

            var result = await _tokenManagementService.GenerateTokensAsync(userId.Value);

            return Ok(result.Value);   
        }

        private async Task<Microsoft.AspNetCore.Identity.SignInResult> AuthenticateUser(LoginUserDto user)
        {
            return await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserDto user)
        {
            var result = await _userService.CreateUserAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto refreshTokenDto)
        {
            await _tokenManagementService.DeleteRefreshTokenAsync(refreshTokenDto.RefreshToken, needHash: true);

            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenDto)
        {
            var refreshTokenFromDB = await _tokenManagementService.GetRefreshTokenAsync(refreshTokenDto.RefreshToken, needHash: true);
            if (!refreshTokenFromDB.Succeeded)
            {
                return Unauthorized(refreshTokenFromDB.Error);
            }
            var checkIfTokenExpired = _tokenManagementService.CheckRefreshTokenExpiration(refreshTokenFromDB.Value);
            if (!checkIfTokenExpired.Succeeded)
            {
                return Unauthorized(checkIfTokenExpired.Error);
            }

            var result = await _tokenManagementService.GenerateTokensAsync(refreshTokenFromDB.Value, UserId);

            return Ok(result.Value);
        }

        [HttpDelete("revoke-tokens")]
        public async Task<IActionResult> RevokeRefreshTokens()
        {
            await _tokenManagementService.RevokeRefreshTokensAsync(UserId);

            return Ok();
        }
    }
}
