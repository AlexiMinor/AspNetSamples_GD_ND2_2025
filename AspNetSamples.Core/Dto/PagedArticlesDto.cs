namespace AspNetSamples.Core.Dto
{
    public class PagedArticlesDto
    {
        public ArticleDto[] Articles { get; set; }
        public int TotalArticles { get; set; }
    }
}
