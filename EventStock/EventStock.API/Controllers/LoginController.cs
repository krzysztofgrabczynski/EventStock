using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventStock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenManagementService _tokenManagementService;
        private readonly IUserService _userService;
        public LoginController(SignInManager<User> signInManager, ITokenManagementService tokenManagementService, IUserService userService)
        {
            _signInManager = signInManager;
            _tokenManagementService = tokenManagementService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var checkUserResult = AuthenticateUser(loginUserDto);
            if(checkUserResult.Result.Succeeded)
            {
                var userId = await _userService.GetUserIdByEmailAsync(loginUserDto.Email);
                var jwtToken = _tokenManagementService.GenerateJWT(userId, loginUserDto.Email);
                return Ok(new { token = jwtToken });
            }

            return BadRequest("Invalid login credentials");
        }

        private async Task<Microsoft.AspNetCore.Identity.SignInResult> AuthenticateUser(LoginUserDto user)
        {
            return await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);
        }
    }
}
