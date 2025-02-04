using Microsoft.AspNetCore.Authorization;

namespace EventStock.Application.Authorization
{
    public class IsUserAdminRequirement : IAuthorizationRequirement
    {
        public IsUserAdminRequirement() { }
    }
}
