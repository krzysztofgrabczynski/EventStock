using Microsoft.AspNetCore.Authorization;

namespace EventStock.Application.Authorization
{
    public class IsStockUserRequirement : IAuthorizationRequirement
    {
        public readonly string RequiredRole = string.Empty;

        public IsStockUserRequirement() { }
        public IsStockUserRequirement(string requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}
