﻿using EventStock.Application.Dto.RefreshToken;
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
        private readonly IJwtTokentService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserService _userService;

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
            var checkUserAutchentication = await AuthenticateUser(loginUserDto);
            if(checkUserAutchentication.Succeeded)
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
            var refreshTokenFromDB = await _refreshTokenService.GetRefreshTokenAsync(refreshTokenDto.RefreshToken, needHash: true);
            if (refreshTokenFromDB == null || !_refreshTokenService.CheckRefreshToken(refreshTokenFromDB))
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }

            var newRefreshToken = await _refreshTokenService.UpdateRefreshTokenAsync(refreshTokenFromDB);
            var newJwtToken = _jwtTokenService.GenerateJWT(UserId);

            return Ok(new
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpDelete("revoke-tokens")]
        public async Task<IActionResult> RevokeRefreshTokens()
        {
            await _refreshTokenService.RevokeRefreshTokensAsync(UserId);

            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserDto user)
        {
            var result = await _userService.CreateUserAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }
        }
    }
}
