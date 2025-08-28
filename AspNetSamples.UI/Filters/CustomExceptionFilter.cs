using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetSamples.UI.Filters;

public class CustomExceptionFilter: Attribute, IExceptionFilter
{
    private readonly ILogger<CustomExceptionFilter> _logger;

    public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var action = context.ActionDescriptor.DisplayName;
        var exceptionStack = context.Exception.StackTrace;
        var exceptionMessage = context.Exception.Message;

        var myCustomSpecificException = new Exception(exceptionMessage, context.Exception);

        context.Result = new ContentResult { Content = "An unexpected error occurred.", StatusCode = 500 };
    }
}