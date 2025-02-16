using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Application.Services;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _userService.GetUserAsync(UserId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserDto user)
        {
            var result = await _userService.UpdateUserAsync(UserId, user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }

            return Ok();
        }

        [HttpDelete("delete-profile")]
        public async Task<IActionResult> DeleteMyProfile()
        {
            var result = await _userService.DeleteUserAsync(UserId);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }

            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordDto changeUserPasswordDto)
        {
            var result = await _userService.UpdateUserPasswordAsync(UserId, changeUserPasswordDto);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);   
            }

            return Ok();
        }

        [HttpGet("my-stock")]
        public async Task<IActionResult> GetMyStock()
        {
            var result = await _userService.GetUserStockAsync(UserId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
