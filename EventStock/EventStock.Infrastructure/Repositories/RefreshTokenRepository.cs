using EventStock.Domain.Interfaces;
using EventStock.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventStock.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly Context _context;
        public RefreshTokenRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteRefreshTokenAsync(string userId)
        {
           await _context.RefreshTokens.Where(t => t.UserId == userId).ExecuteDeleteAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
