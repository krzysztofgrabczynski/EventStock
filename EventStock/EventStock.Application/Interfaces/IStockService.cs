using EventStock.Application.Dto.Event;
using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using EventStock.Application.ResultPattern;

namespace EventStock.Application.Interfaces
{
    public interface IStockService
    {
        Task<Result<ViewStockDto>> GetStockAsync(int id);
        Task<Result> UpdateStockAsync(int id, UpdateStockDto stockDto);
        Task<Result> DeleteStockAsync(int id);

        Task<Result> AddUserAsync(int stockId, string email, string roleName);
        Task<Result> DeleteUserAsync(int stockId, string userId);
        Task<Result<List<UserDto>>> ListUsersByStockIdAsync(int id);
        Task<Result> UpdateUserRoleAsync(int stockId, string email, string roleName);
        
        Task<List<ViewEventDto>> ListStockEventsAsync(int id);
        Task<List<ViewEventDto>> ListStockEventsAsync(int id, DateTime date);
    }
}
