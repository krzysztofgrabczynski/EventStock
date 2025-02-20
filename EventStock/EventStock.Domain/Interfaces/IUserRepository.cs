using EventStock.Domain.Models;

namespace EventStock.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Stock?> GetUserStockAsync(string userId);
        Task<bool> EmailExistInDB(string userEmail);
    }
}
