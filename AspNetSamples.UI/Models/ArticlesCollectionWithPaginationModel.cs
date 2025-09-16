using AspNetSamples.Core.Dto;

namespace AspNetSamples.UI.Models;

public class ArticlesCollectionWithPaginationModel
{
    public bool IsUserAdmin { get; set; }
    public ArticleDto[] Articles { get; set; } 
    public PagingInfoModel PagingInfo { get; set; }
}