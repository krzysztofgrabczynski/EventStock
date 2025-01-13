using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventStock.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtTokentService _jwtTokentService;
        private readonly IUserService _userService;

        public UserController(IJwtTokentService jwtTokentService, IUserService userService)
        {
            _jwtTokentService = jwtTokentService;
            _userService = userService;
        }

        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = _jwtTokentService.GetIdFromJwtToken(HttpContext.Request.Headers);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid access token" });
            }

            var user = await _userService.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserDto user)
        {
            var userId = _jwtTokentService.GetIdFromJwtToken(HttpContext.Request.Headers);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid access token" });
            }

            var result = await _userService.UpdateUserAsync(userId, user);
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
            var userId = _jwtTokentService.GetIdFromJwtToken(HttpContext.Request.Headers);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid access token" });
            }

            var result = await _userService.DeleteUserAsync(userId);
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
            var userId = _jwtTokentService.GetIdFromJwtToken(HttpContext.Request.Headers);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid access token" });
            }

            var result = await _userService.UpdateUserPasswordAsync(userId, changeUserPasswordDto);
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
