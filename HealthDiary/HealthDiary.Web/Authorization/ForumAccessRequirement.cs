using Microsoft.AspNetCore.Authorization;

namespace HealthDiary.Web.Authorization
{
    public class ForumAccessRequirement : IAuthorizationRequirement
    {
    }

    public class ForumAccessHandler : AuthorizationHandler<ForumAccessRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ForumAccessRequirement requirement)
        {
            if (context.User.HasClaim(c =>
                c.Type == "IsMentor" ||
                c.Type == "IsVerifiedUser" ||
                c.Type == "HasForumAccess"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}