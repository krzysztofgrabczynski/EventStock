using Microsoft.AspNetCore.Authorization;

namespace EventStock.Application.Authorization
{
    public class IsStockUserRequirement : IAuthorizationRequirement
    {
        public IsStockUserRequirement() { }
    }
}
