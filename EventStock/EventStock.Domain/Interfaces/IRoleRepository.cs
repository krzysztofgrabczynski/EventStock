using Microsoft.AspNetCore.Identity;

namespace EventStock.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task <IdentityRole?> GetRoleByNameAsync(string roleName);
    }
}
