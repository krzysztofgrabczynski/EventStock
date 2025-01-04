using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;


namespace EventStock.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserDto userDto);
        Task<UserDto> GetUserAsync(int id);
        Task<UserDto> UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int id);
        Task<List<StockDto>> ListUsersStocksAsync(int id);
    }
}
