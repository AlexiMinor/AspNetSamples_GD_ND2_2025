using AspNetSamples.Core.Dto;
using System.ServiceModel.Syndication;
using System.Xml;

namespace AspNetSamples.Services.Abstractions;

public interface ISyndicationFeedReader
{
    public SyndicationFeed GetSyndicationFeed(XmlReader? reader);
}

//not a correct way to do this, but System.ServiceModel.Syndication.SyndicationFeed.Load is internal in net8.0 & static