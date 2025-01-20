using EventStock.Domain.Models;

namespace EventStock.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<Stock>> ListUsersStocksAsync(string userId);
    }
}
