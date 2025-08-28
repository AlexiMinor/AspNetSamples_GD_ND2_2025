using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetSamples.UI.Filters;

public class CustomResponseResourceFilter : Attribute, IResourceFilter
{
    //private readonly string _userName;
    private readonly ILogger<CustomResponseResourceFilter> _logger;
    public CustomResponseResourceFilter(ILogger<CustomResponseResourceFilter> logger)
    {
        _logger = logger;
        //_userName = userName;
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        _logger.LogInformation("CustomResponseResourceFilter executing");
        context.Result = new ContentResult
        {
            Content = $"Custom response from resource filter for",
        };
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        
    }
}