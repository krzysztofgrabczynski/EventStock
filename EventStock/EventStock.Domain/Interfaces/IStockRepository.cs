using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace EventStock.Infrastructure.Repositories
{
    public interface IStockRepository
    {
        Task<int?> CreateStockAsync(Stock stock);
        Task DeleteStockAsync(Stock stock);
        Task<Stock?> GetStockAsync(int id);
        Task UpdateStockAsync(Stock stock);
        Task<bool> AddUserAsync(Stock stock, User user, IdentityRole role);
        Task<bool> UpdateUserRoleAsync(Stock stock, User user, IdentityRole role);
        Task DeleteUserAsync(Stock stock, User user);
    }
}
