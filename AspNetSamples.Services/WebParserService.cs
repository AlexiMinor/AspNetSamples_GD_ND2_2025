using AspNetSamples.Core.Dto;
using AspNetSamples.Services.Abstractions;
using HtmlAgilityPack;
using System;
using Microsoft.Extensions.Logging;

namespace AspNetSamples.Services;

public class WebParserService : IWebParserService
{
    private readonly IHtmlCleanerService _htmlCleanerService;
    private readonly ILogger<WebParserService> _logger;

    public WebParserService(IHtmlCleanerService htmlCleanerService,
        ILogger<WebParserService> logger)
    {
        _htmlCleanerService = htmlCleanerService;
        _logger = logger;
    }

    public async Task<string> ParseArticlesAsync(string url, CancellationToken cancellationToken)
    {
        try
        {
            var web = new HtmlWeb();
            var html = string.Empty;
            //if (url.Contains("onliner.by"))
            //{
            var htmlDoc = await web.LoadFromWebAsync(url, cancellationToken);

            html = ParseOnlinerArticle(htmlDoc);
            _logger.LogDebug($"Article {url} parsed");

            var articleTextWithP = _htmlCleanerService.CleanHtmlAttributes(html);
            _logger.LogDebug($"Article {url} cleaned from attributes");
            var articleText = _htmlCleanerService.CleanHtml(html);
            _logger.LogDebug($"Article {url} cleaned from tags");
            return articleTextWithP;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error during article {url} web scrapping");
            throw;
        }
    }

    private string ParseOnlinerArticle(HtmlDocument htmlDoc)
    {
        try
        {
            var data = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");

            var socialNetworkNode = data.SelectSingleNode(".//div[@class='news-reference']");
            if (socialNetworkNode != null)
            {
                socialNetworkNode.ParentNode.RemoveChild(socialNetworkNode);
            }

            var scriptNodes = data.SelectNodes(".//script");
            if (scriptNodes.Any())
            {
                foreach (var scriptNode in scriptNodes)
                {
                    scriptNode.ParentNode.RemoveChild(scriptNode);
                }
            }

            var adWidget = data.SelectSingleNode(".//div[@class='news-widget']");
            if (adWidget != null)
            {
                adWidget.ParentNode.RemoveChild(adWidget);
            }

            var nonArticleParagraph = data.SelectSingleNode(".//p[last()]");
            if (nonArticleParagraph != null)
            {
                nonArticleParagraph.ParentNode.RemoveChild(nonArticleParagraph);
            }

            var htmlArticleText = data.InnerHtml;
            return htmlArticleText;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during Onliner article parsing");
            throw;
        }


    }
}