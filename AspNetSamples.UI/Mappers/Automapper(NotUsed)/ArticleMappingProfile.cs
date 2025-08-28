using AspNetSamples.Core.Dto;
using AspNetSamples.Database.Entities;
using AutoMapper;

namespace AspNetSamples.UI.Mappers.Automapper_NotUsed_;

public class ArticleMappingProfile : Profile
{
    public ArticleMappingProfile()
    {
        CreateMap<Article, ArticleDto>()
            .ForMember(dest => dest.Id, 
                opt =>
                    opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SourceName, 
                opt =>
                    opt.MapFrom(src => src.Source.Name))
            .ForMember(dest => dest.Rate, 
                opt =>
                    opt.MapFrom(src => src.Rate ?? 0.0));

        //incorrect
        CreateMap<ArticleDto, Article>()
            .ForMember(dest => dest.Source, opt => opt.Ignore());
    }
}