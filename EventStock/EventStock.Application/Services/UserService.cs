using AutoMapper;
using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using EventStock.Application.ResultPattern;
using EventStock.Application.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;
using EventStock.Application.ResultPattern.Errors;
using EventStock.Domain.Interfaces;

namespace EventStock.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UserService(IMapper mapper, UserManager<User> userManager, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
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
            mappedUser.Roles = [.. (await _userManager.GetRolesAsync(user))];

            return Result<UserDto>.Success(mappedUser);
        }

        public async Task<Result<User>> GetUserModelAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Result<User>.Failure(new UserNotFoundResultError());
            }

            return Result<User>.Success(user);
        }

        public async Task<Result<User>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<User>.Failure(new UserWithEmailNotFoundResultError());
            }
            return Result<User>.Success(user);
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

            var deleteResult = await _userManager.DeleteAsync(user);
            if (deleteResult.Succeeded)
            {
                await _refreshTokenRepository.RevokeRefreshTokensAsync(id);
            }

            return deleteResult;
        }

        public async Task<Result<ViewStockDto>> GetUserStockAsync(string userId)
        {
            var result = await _userRepository.GetUserStockAsync(userId);
            if (result == null)
            {
                return Result<ViewStockDto>.Failure(new StockDoesNotExistResultError());
            }

            var mappedStock =  _mapper.Map<ViewStockDto>(result);

            return Result<ViewStockDto>.Success(mappedStock);
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
