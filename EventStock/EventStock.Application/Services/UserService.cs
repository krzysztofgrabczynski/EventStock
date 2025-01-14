using AutoMapper;
using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using EventStock.Application.ResultPattern;
using EventStock.Application.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;
using EventStock.Application.ResultPattern.Errors;

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

        public async Task<Result<UserDto>> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Result<UserDto>.Failure(new UserNotFoundResultError());
            }
            var mappedUser = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(mappedUser);
        }
        public async Task<string?> GetUserIdByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return user.Id;
        }

        public async Task<IdentityResult> UpdateUserAsync(string id, UserDto updatedUser)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(new UserNotFoundIdentityError());
            }
            user.Email = updatedUser.Email ?? user.Email;
            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.LastName = updatedUser.LastName ?? user.LastName;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateUserPasswordAsync(string id, ChangeUserPasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(new UserNotFoundIdentityError());
            }

            return await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.Password);
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(new UserNotFoundIdentityError());
            }

            return await _userManager.DeleteAsync(user);
        }

        public Task<List<StockDto>> ListUsersStocksAsync(int id)
        {
            throw new NotImplementedException();
        }    

        private class UserNotFoundIdentityError : IdentityError
        {
            public UserNotFoundIdentityError()
            {
                Code = "UserNotFound"; 
                Description = "User with provided ID was not found";
            }
        }
    }
}
