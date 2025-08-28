using AspNetSamples.Core.Dto;
using AspNetSamples.Database.Entities;
using AspNetSamples.Models;
using Riok.Mapperly.Abstractions;

namespace AspNetSamples.Mappers;

[Mapper]
public partial class SourceMapper
{

    public partial SourceDto MapSourceToSourceDto(Source article);

    public partial Source MapSourceDtoToSource(SourceDto dto);


}