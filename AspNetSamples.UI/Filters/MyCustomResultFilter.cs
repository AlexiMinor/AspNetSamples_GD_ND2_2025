using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetSamples.UI.Filters;

public class MyCustomResultFilter: Attribute, IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        // Before the action result executes
        context.HttpContext.Response.Headers.Add("X-Custom-Header", "MyCustomValue");
    }
    public void OnResultExecuted(ResultExecutedContext context)
    {
        // After the action result executes
    }
}