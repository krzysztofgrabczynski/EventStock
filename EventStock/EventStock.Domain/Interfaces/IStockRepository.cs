using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace EventStock.Infrastructure.Repositories
{
    public interface IStockRepository
    {
        Task AddUserAsync(Stock stock, User user, IdentityRole role);
        Task<int?> CreateStockAsync(Stock stock);
        Task<Stock?> GetStockAsync(int id);
    }
}
