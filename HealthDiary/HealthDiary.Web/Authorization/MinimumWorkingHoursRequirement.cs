using Microsoft.AspNetCore.Authorization;

namespace HealthDiary.Web.Authorization
{
    public class MinimumWorkingHoursRequirement : IAuthorizationRequirement
    {
        public int MinimumHours { get; }

        public MinimumWorkingHoursRequirement(int minimumHours)
        {
            MinimumHours = minimumHours;
        }
    }

    public class MinimumWorkingHoursHandler : AuthorizationHandler<MinimumWorkingHoursRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MinimumWorkingHoursRequirement requirement)
        {
            var hoursClaim = context.User.FindFirst("WorkingHours");
            if (hoursClaim != null && int.TryParse(hoursClaim.Value, out int hours) && hours >= requirement.MinimumHours)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}