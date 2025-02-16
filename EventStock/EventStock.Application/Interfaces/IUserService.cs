using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using EventStock.Application.ResultPattern;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;


namespace EventStock.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(CreateUserDto userDto);
        Task<Result<UserDto>> GetUserAsync(string id);
        Task<Result<User>> GetUserModelAsync(string id);
        Task<Result<User>> GetUserByEmailAsync(string email);
        Task<IdentityResult> UpdateUserAsync(string id, UserDto updatedUser);
        Task<IdentityResult> UpdateUserPasswordAsync(string id, ChangeUserPasswordDto changePasswordDto);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<Result<ViewStockDto>> GetUserStockAsync(string id);
    }
}
