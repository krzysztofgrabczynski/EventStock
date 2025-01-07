using AutoMapper;
using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Domain.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.AccessControl;

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

        public Task<UserDto> UpdateUserAsync(UserDto user)
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
