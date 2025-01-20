using EventStock.Application.Dto.Equipment;
using EventStock.Application.Dto.Event;
using EventStock.Application.Dto.Stock;
using EventStock.Application.ResultPattern;
using Microsoft.AspNetCore.Identity;

namespace EventStock.Application.Interfaces
{
    public interface IStockService
    {
        Task<Result<int>> CreateStockAsync(CreateStockDto stock);
        Task<Result<ViewStockDto>> GetStockAsync(int id);
        Task<ViewStockDto> UpdateStockAsync(StockDto stock);
        Task DeleteStockAsync(int id);

        Task <Result> AddUserAsync(int stockId, string userId);
        Task DeleteUserAsync(int id);
        Task<List<StockUserDto>> ListUsersByStockIdAsync(int id);
        Task AddRoleToStockUserAsync(int id, IdentityRole role);
        
        Task<List<ViewEquipmentDto>> ListStockEquipmentAsync(int id);
        Task<List<ViewEquipmentDto>> ListAvailableStockEquipmentInDateAsync(int id, DateTime date);
        Task<List<ViewEventDto>> ListStockEventsAsync(int id);
        Task<List<ViewEventDto>> ListStockEventsAsync(int id, DateTime date);
    }
}
