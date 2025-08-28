using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetSamples.UI.Filters;

public class WhiteSpaceRemoverActionFilter: Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var response = context.HttpContext.Response;
        var htmlStream = response.Body;
        
        using (var reader = new StreamReader(htmlStream))
        {
            //remove whitespace from htmlStream and write back to response body
            var html = reader.ReadToEnd();
            var htmlWithoutWhitespace = Regex.Replace(html, @"\s+", " ");//change regex and test it
            var bytes = Encoding.UTF8.GetBytes(htmlWithoutWhitespace);
            response.Body.Seek(0, SeekOrigin.Begin);
            response.Body.Write(bytes, 0, bytes.Length);
        }

    }
    
    
    
}