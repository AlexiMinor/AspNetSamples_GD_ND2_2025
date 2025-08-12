using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetSamples.UI.Helpers;

[HtmlTargetElement("div", Attributes = "datetime", ParentTag = "main")]
public class DateTimeTagHelper : TagHelper
{

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var data =await output.GetChildContentAsync();

        output.TagName = "div";
        output.Content.SetContent(DateTime.Now.ToString("R"));
        var data2 = output.Content.GetContent();

    }
}