using AspNetSamples.Services.Abstractions;
using System.ServiceModel.Syndication;
using System.Xml;

namespace AspNetSamples.Services;

public class SyndicationFeedReader : ISyndicationFeedReader
{
    public SyndicationFeed GetSyndicationFeed(XmlReader? reader)
    {
        return SyndicationFeed.Load(reader);
    }
}