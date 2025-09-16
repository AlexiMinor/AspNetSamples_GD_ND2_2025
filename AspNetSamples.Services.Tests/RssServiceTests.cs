using System.ServiceModel.Syndication;
using System.Xml;
using AspNetSamples.Core.Dto;
using AspNetSamples.Mappers;
using AspNetSamples.Services.Abstractions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AspNetSamples.Services.Tests
{
    public class RssServiceTests
    {
        private ArticleMapper _articleMapper;
        private ILogger<RssService> _loggerMock;
        private ISyndicationFeedReader _syndicationFeedReader;

        private RssService ConfigureRssService()
        {
            _articleMapper = new ArticleMapper();
            _loggerMock = Substitute.For<ILogger<RssService>>();
            _syndicationFeedReader = Substitute.For<ISyndicationFeedReader>();

            return new RssService(_articleMapper, _loggerMock, _syndicationFeedReader);
        }

        [Fact]
        public async Task GetRssFeedBySourceAsync_ReturnsValidFeed()
        {
            // Arrange
            var service = ConfigureRssService();

            var existingArticleUrls = new HashSet<string>
            {
                "https://example.com/article1",
                "https://example.com/article2"
            };

            _syndicationFeedReader
                .GetSyndicationFeed(Arg.Any<XmlReader>())
                .Returns(
                    new SyndicationFeed()
                    {
                        Items = new List<SyndicationItem>()
                        {
                            new SyndicationItem()
                            {
                                Id = "https://example.com/article1"
                            },
                            new SyndicationItem()
                            {
                                Id = "https://example.com/article2"
                            },
                            new SyndicationItem()
                            {
                                Id = "https://example.com/article3"
                            },
                        }
                    });

            var source = new SourceDto { RssLink = "https://www.onliner.by/feed" };
          
            // Act
            var result = await service.GetRssFeedBySourceAsync(source, existingArticleUrls);

            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GetRssFeedBySourceAsync_WhenNoItems_ReturnsEmptyFeed()
        {    // Arrange
            var service = ConfigureRssService();
            var source = new SourceDto { RssLink = "https://www.onliner.by/feed" };
            var existingArticleUrls = new HashSet<string>
            {
                "https://example.com/article1",
                "https://example.com/article2"
            };
            
            _syndicationFeedReader
                .GetSyndicationFeed(Arg.Any<XmlReader>())
                .Returns((SyndicationFeed?)null);

            // Act
            var result = await service.GetRssFeedBySourceAsync(source, existingArticleUrls);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetRssFeedBySourceAsync_WhenNoNewItems_ReturnsEmptyFeed()
        {
            // Arrange
            var service = ConfigureRssService();
            var source = new SourceDto { RssLink = "https://www.onliner.by/" };
            var existingArticleUrls = new HashSet<string>
            {
                "https://example.com/article1",
                "https://example.com/article2"
            };

            _syndicationFeedReader
                .GetSyndicationFeed(Arg.Any<XmlReader>())
                .Returns(
                    new SyndicationFeed()
                    {
                        Items = new List<SyndicationItem>()
                        {
                            new()
                            {
                                Id = "https://example.com/article1"
                            },
                            new()
                            {
                                Id = "https://example.com/article2"
                            }
                        }
                    });

            // Act
            var result = await service.GetRssFeedBySourceAsync(source, existingArticleUrls);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetRssFeedBySourceAsync_WhenInvalidUrl_ThrowsException()
        {
            // Arrange
            var service = ConfigureRssService();
            var source = new SourceDto { RssLink = "https://www.example.com/feed" };
            var existingArticleUrls = new HashSet<string>
            {
                "https://example.com/article1",
                "https://example.com/article2"
            };
            
            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => { await service.GetRssFeedBySourceAsync(source, existingArticleUrls); });
        }
    }
   
}