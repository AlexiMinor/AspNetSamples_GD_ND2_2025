namespace AspNetSamples.Services.Abstractions;

public interface IHtmlCleanerService
{
    public string? CleanHtmlAttributes(string rawHtml);
    
    public string? CleanHtml(string rawHtml);
}
