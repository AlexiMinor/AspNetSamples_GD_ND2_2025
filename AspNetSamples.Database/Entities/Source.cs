namespace AspNetSamples.Database.Entities;

public class Source
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DomainName { get; set; }
    public string? RssLink { get; set; }
    
    public List<Article> Articles{ get; set; }
    
}