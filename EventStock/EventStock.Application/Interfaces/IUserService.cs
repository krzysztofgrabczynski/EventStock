using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using Microsoft.AspNetCore.Identity;


namespace EventStock.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto> GetUserAsync(string id);
        Task<UserDto> UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int id);
        Task<List<StockDto>> ListUsersStocksAsync(int id);
    }
}
