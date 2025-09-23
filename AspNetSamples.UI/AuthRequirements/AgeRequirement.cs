using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AspNetSamples.UI.AuthRequirements;

public class AgeRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }

    public AgeRequirement(int minimumAge) => MinimumAge = minimumAge;
}

public class MinAgeHandler : AuthorizationHandler<AgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {

        var dateOfBirthClaim = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);
        if (dateOfBirthClaim is not null)
        {
            var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
            var calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            
            if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
                calculatedAge--;

            if (calculatedAge >= requirement.MinimumAge)
                context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}