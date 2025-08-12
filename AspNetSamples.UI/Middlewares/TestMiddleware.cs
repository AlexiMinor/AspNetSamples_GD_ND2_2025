using AspNetSamples.Services.Abstractions;

namespace AspNetSamples.UI.Middlewares;

public class TestMiddleware
{
    private readonly RequestDelegate _next;

    public TestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IArticleService service)
    {
        
        //var result = await service.GetArticlesByPageAsync();

        var testData = context.Request.Query["adv"];
        if (!string.IsNullOrWhiteSpace(testData.ToString()))
        {
            await context.Response.WriteAsync("adv");
        }

        await _next.Invoke(context);
    }
}