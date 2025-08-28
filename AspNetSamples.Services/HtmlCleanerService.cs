using AspNetSamples.Services.Abstractions;

namespace AspNetSamples.Services;

public class HtmlCleanerService : IHtmlCleanerService
{
    public string? CleanHtmlAttributes(string rawHtml)
    {
        if (string.IsNullOrWhiteSpace(rawHtml)) return rawHtml;

        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
        htmlDocument.LoadHtml(rawHtml);
        
        var mediaNodes = htmlDocument.DocumentNode.Descendants()
            .Where(n => n.Name is "img" or "video" or "audio" or "source" or "iframe" or "picture" or "svg" or "math");

        foreach (var node in htmlDocument.DocumentNode.Descendants()
                     .Where(n => !mediaNodes.Contains(n)))
        {
            if (node.Attributes is { Count: > 0 })
            {
                node.Attributes.RemoveAll();
            }
        }
        //remove all empty tags
        var emptyTags = htmlDocument.DocumentNode.ChildNodes
            .Where(n => !mediaNodes.Contains(n))
            .Where(n => string.IsNullOrWhiteSpace(n.InnerText) && !n.HasChildNodes)
            .ToArray();

        foreach (var tag in emptyTags)
        {
            if (htmlDocument.DocumentNode.ChildNodes.Contains(tag))
            {
                htmlDocument.DocumentNode.RemoveChild(tag);
            }
        }

        return htmlDocument.DocumentNode.OuterHtml;
    }

    public string? CleanHtml(string rawHtml)
    {
        if (string.IsNullOrWhiteSpace(rawHtml)) return rawHtml;
        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
        htmlDocument.LoadHtml(rawHtml);
        return htmlDocument.DocumentNode.InnerText;
    }
}