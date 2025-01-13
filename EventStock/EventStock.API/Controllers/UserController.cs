﻿using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventStock.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtTokentService _jwtTokentService;
        private readonly IUserService _userService;
        private string UserId 
        {
            get
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("Invalid access token");
                }
                return userId;
            } 
        }

        public UserController(IJwtTokentService jwtTokentService, IUserService userService)
        {
            _jwtTokentService = jwtTokentService;
            _userService = userService;
        }

        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var user = await _userService.GetUserAsync(UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserDto user)
        {
            var result = await _userService.UpdateUserAsync(UserId, user);
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

        [HttpDelete("delete-profile")]
        public async Task<IActionResult> DeleteMyProfile()
        {
            var result = await _userService.DeleteUserAsync(UserId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordDto changeUserPasswordDto)
        {
            var result = await _userService.UpdateUserPasswordAsync(UserId, changeUserPasswordDto);
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
