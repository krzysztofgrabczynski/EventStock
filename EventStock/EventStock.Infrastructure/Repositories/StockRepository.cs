using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventStock.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public StockRepository(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> AddUserAsync(Stock stock, User user, IdentityRole role)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _userManager.AddToRoleAsync(user, role.Name!);
                stock.Users.Add(user);
                var userStockRole = new UserStockRole()
                {
                    User = user,
                    Stock = stock,
                    Role = role
                };
                await _context.UserStockRoles.AddAsync(userStockRole);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
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

        public async Task UpdateStockAsync(Stock stock)
        {
            _context.Stocks.Update(stock);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStockAsync(Stock stock)
        {
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Stock stock, User user)
        {
            stock.Users.Remove(user);
            await _context.UserStockRoles.Where(u => u.User == user && u.Stock == stock).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task<Stock?> GetStockAsync(int id)
        {
            return await _context.Stocks
                .Include(s => s.Users)
                .Include(s => s.UserStockRoles)
                    .ThenInclude(usr => usr.Role)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> UpdateUserRoleAsync(Stock stock, User user, IdentityRole role)
        {
            var userStockRole = await _context.UserStockRoles.FirstOrDefaultAsync(u => 
                u.Stock == stock && u.User == user);

            if (userStockRole == null)
            {
                return false;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (!await UpdateIdentityRoleAsync(userStockRole.Role.Name!, role!.Name!, user))
                {
                    throw new InvalidOperationException();
                }

                userStockRole.Role = role ?? userStockRole.Role;
                _context.UserStockRoles.Update(userStockRole);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        private async Task<bool> UpdateIdentityRoleAsync(string oldRole, string newRole, User user)
        {
            if (await _userManager.IsInRoleAsync(user, oldRole))
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(user, oldRole);
                if (!removeResult.Succeeded)
                {
                    return false;
                }
            }
            
            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            return addResult.Succeeded;            
        }
    }
}
