using AspNetSamples.Core.Dto;

namespace AspNetSamples.UI.Models;

public class ArticlesCollectionWithPaginationModel
{
    public List<ArticleDto> Articles { get; set; } 
    public PagingInfoModel PagingInfo { get; set; }
}