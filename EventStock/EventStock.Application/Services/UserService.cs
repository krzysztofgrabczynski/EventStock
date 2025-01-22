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

        public UserService(IMapper mapper, UserManager<User> userManager, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userRepository = userRepository;
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

        public async Task<Result<string>> GetUserIdByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<string>.Failure(new UserWithEmailNotFoundResultError());
            }
            return Result<string>.Success(user.Id);
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

        public async Task<List<ViewStockDtoForList>> ListUsersStocksAsync(string userId)
        {
            var result = await _userRepository.ListUsersStocksAsync(userId);
            var mappedStocksList = new List<ViewStockDtoForList>();
            foreach (var stock in result)
            {
                mappedStocksList.Add(_mapper.Map<ViewStockDtoForList>(stock));
            }

            return mappedStocksList;
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
