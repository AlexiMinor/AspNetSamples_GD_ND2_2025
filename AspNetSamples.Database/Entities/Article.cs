namespace AspNetSamples.Database.Entities;

public class Article
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Content { get; set; }
    public string OriginUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public double? Rate { get; set; }
    public Guid SourceId { get; set; }
    public Source? Source { get; set; }
    
}