using AspNetSamples.Core.Dto;
using AspNetSamples.Models;

namespace AspNetSamples.UI.Models;

public class EditArticleViewModel
{
  public EditArticleModel EditModel { get; set; } 
  public IEnumerable<SourceDto> Sources { get; set; }

}