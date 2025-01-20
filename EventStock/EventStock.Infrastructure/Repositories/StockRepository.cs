using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventStock.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly Context _context;

        public StockRepository(Context context)
        {
            _context = context;
        }

        public async Task AddUserAsync(Stock stock, User user, IdentityRole role)
        {
            stock.Users.Add(user);
            var userStockRole = new UserStockRole()
            {
                User = user,
                Stock = stock,
                Role = role
            };

            await _context.UserStockRoles.AddAsync(userStockRole);
            var result = await _context.SaveChangesAsync();    
        }

        public async Task<int?> CreateStockAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {   
                return stock.Id;
            }
            return null;
        }

        public async Task<Stock?> GetStockAsync(int id)
        {
            return await _context.Stocks
                .Include(s => s.Users)
                .Include(s => s.UserStockRoles)
                    .ThenInclude(usr => usr.Role)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
