using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using Microsoft.AspNetCore.Identity;


namespace EventStock.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto> GetUserAsync(string id);
        Task<string?> GetUserIdByEmailAsync(string email);
        Task<IdentityResult> UpdateUserAsync(string id, UserDto updatedUser);
        Task<IdentityResult> UpdateUserPasswordAsync(string id, ChangeUserPasswordDto changePasswordDto);
        Task<bool> DeleteUserAsync(string id);
        Task<List<StockDto>> ListUsersStocksAsync(int id);
    }
}
