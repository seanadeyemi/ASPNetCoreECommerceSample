using ASPNetCoreECommerceSample.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASPNetCoreECommerceSample.Handlers
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;
            }

            var dateOfBirth = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);

            var age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth > DateTime.Today.AddYears(age))
            {
                age--;
            }

            if (age >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
