using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetSamples.UI.Filters;

public class LastVisitResourceFilter : Attribute, IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        context.HttpContext.Response.Cookies.Append("LastVisit", DateTime.UtcNow.ToString("O"));
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        
    }

    
}