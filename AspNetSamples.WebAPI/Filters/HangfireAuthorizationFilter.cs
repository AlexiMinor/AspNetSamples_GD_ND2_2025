using Hangfire.Dashboard;

namespace AspNetSamples.WebAPI.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;
        return  isAuthenticated && httpContext.User.IsInRole("Admin");
    }
}