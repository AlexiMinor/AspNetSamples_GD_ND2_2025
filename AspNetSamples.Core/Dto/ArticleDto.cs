namespace AspNetSamples.Core.Dto
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string SourceName { get; set; }
        public Guid SourceId { get; set; }
        public double Rate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
