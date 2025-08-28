using AspNetSamples.Services.Abstractions;

namespace AspNetSamples.UI.Middlewares;

public class ExceptionLoggerMiddleware 
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionLoggerMiddleware> _logger;

    public ExceptionLoggerMiddleware(RequestDelegate next, ILogger<ExceptionLoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public void Invoke(HttpContext context)
    {
       
    }
}