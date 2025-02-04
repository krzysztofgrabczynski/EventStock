using EventStock.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EventStock.Application.Authorization
{
    public class IsUserAdminHandler : AuthorizationHandler<IsUserAdminRequirement>
    {
        private readonly UserManager<User> _userManager;

        public IsUserAdminHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsUserAdminRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                context.Fail();
                return;
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
    }
}
