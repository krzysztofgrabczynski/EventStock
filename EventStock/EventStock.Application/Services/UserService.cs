using AutoMapper;
using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace EventStock.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserService(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserDto userDto)
        {
            var mappedUser = _mapper.Map<User>(userDto);
            return await _userManager.CreateAsync(mappedUser, userDto.Password);
        }

        public async Task<UserDto> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var mappedUser = _mapper.Map<UserDto>(user);
            return mappedUser;
        }
        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException($"User with email: {email} not found");
            }
            return user.Id;
        }

        public async Task<IdentityResult> UpdateUserAsync(string id, UserDto updatedUser)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "User with provided ID was not found"
                });
            }
            user.Email = updatedUser.Email ?? user.Email;
            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.LastName = updatedUser.LastName ?? user.LastName;

            return await _userManager.UpdateAsync(user);
        }

        public Task<IdentityResult> UpdateUserPasswordAsync(string id, ChangeUserPasswordDto changePasswordDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public Task<List<StockDto>> ListUsersStocksAsync(int id)
        {
            throw new NotImplementedException();
        }    
    }
}
