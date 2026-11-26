using System.Security.Claims;
using HealthDiary.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace HealthDiary.Web.Authorization
{
    public class IsMaterialOwnerRequirement : IAuthorizationRequirement
    {
    }

    public class IsMaterialOwnerHandler : AuthorizationHandler<IsMaterialOwnerRequirement, Material>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsMaterialOwnerRequirement requirement,
            Material resource)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId) && resource.AuthorId == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}