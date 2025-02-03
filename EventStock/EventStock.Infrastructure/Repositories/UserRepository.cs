using EventStock.Domain.Interfaces;
using EventStock.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventStock.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Stock>> ListUsersStocksAsync(string userId)
        {
            return await _context.Stocks
                .Include(s => s.Address)
                .Where(s => s.Users.Any(u => u.Id == userId))
                .ToListAsync();
        }
    }
}
