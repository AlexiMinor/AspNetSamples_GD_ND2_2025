using AspNetSamples.Core.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetSamples.UI.Models;

public class EditArticleModel
{
    
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Text { get; set; }
    
    public Guid SourceId { get; set; }

    public IEnumerable<SourceDto> Sources { get; set; }
}