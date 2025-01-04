using EventStock.Application.Dto.Equipment;
using EventStock.Application.Dto.Event;
using EventStock.Application.Dto.Stock;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace EventStock.Application.Interfaces
{
    public interface IStockService
    {
        Task<int> CreateStockAsync(StockDto stock);
        Task<ViewStockDto> GetStockAsync(int id);
        Task<ViewStockDto> UpdateStockAsync(StockDto stock);
        Task DeleteStockAsync(int id);

        Task AddUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<List<StockUserDto>> ListUsersByStockIdAsync(int id);
        Task AddRoleToStockUserAsync(int id, IdentityRole role);
        
        Task<List<ViewEquipmentDto>> ListStockEquipmentAsync(int id);
        Task<List<ViewEquipmentDto>> ListAvailableStockEquipmentInDateAsync(int id, DateTime date);
        Task<List<ViewEventDto>> ListStockEventsAsync(int id);
        Task<List<ViewEventDto>> ListStockEventsAsync(int id, DateTime date);
    }
}
