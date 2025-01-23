using EventStock.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventStock.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly Context _context;

        public RoleRepository(Context context)
        {
            _context = context;
        }

        public async Task<IdentityRole?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}
