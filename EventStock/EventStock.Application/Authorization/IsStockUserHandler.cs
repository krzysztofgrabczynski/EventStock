using EventStock.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EventStock.Application.Authorization
{
    public class IsStockUserHandler : AuthorizationHandler<IsStockUserRequirement, Stock>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsStockUserRequirement requirement, Stock resource)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!resource.UserStockRoles.Any(usr => usr.User.Id == userId && usr.Role.Name == "StockAdmin"))
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
