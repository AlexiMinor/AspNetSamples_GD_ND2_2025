using AspNetSamples.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetSamples.UI.Helpers;

[HtmlTargetElement("div", Attributes = "paging-info")]
public class PaginationTagHelper : TagHelper
{
    private IUrlHelperFactory _urlHelperFactory;
    public PagingInfoModel PagingInfo { get; set; }
    public bool PageClassEnabled { get; set; }
    public string Action { get; set; }
    public string PageClass { get; set; }
    public string PageClassSelected { get; set; }
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }


    public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var urHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
        var tagBuilder = new TagBuilder("div");
        var anchorInnerHtml = "";

        for (var i = 1; i <= PagingInfo.TotalPages; i++)
        {
            var tag = new TagBuilder("a");
            anchorInnerHtml = GetAnchorInnerHtml(i, PagingInfo);
            if (anchorInnerHtml.Equals(".."))
            {
                tag.Attributes["href"] = "#";
            }
            else {
                tag.Attributes["href"] = urHelper.Action(Action, new { currentPage = i });
            }

            if (PageClassEnabled)
            {
                tag.AddCssClass(PageClass);
                tag.AddCssClass(i == PagingInfo.CurrentPage ? PageClassSelected : "");
            }
            tag.InnerHtml.Append(anchorInnerHtml);
            if (!string.IsNullOrEmpty(anchorInnerHtml))
            {
                tagBuilder.InnerHtml.AppendHtml(tag);
            }
        }
        output.Content.AppendHtml(tagBuilder.InnerHtml);
    }

    private string GetAnchorInnerHtml(int i, PagingInfoModel model)
    {
        var anchorInnerHtml = "";
        if (model.TotalPages <= 10)
        {
            anchorInnerHtml = i.ToString();
        }
        else
        {
            switch (model.CurrentPage)
            {
                case <= 5 when (i <= 8) 
                               || i == model.TotalPages:
                    anchorInnerHtml = i.ToString();
                    break;
                case <= 5:
                {
                    if (i == model.TotalPages - 1)
                    {
                        anchorInnerHtml = "..";
                    }

                    break;
                }
                case > 5 when (model.TotalPages - model.CurrentPage >= 5):
                {
                    if ((i == 1) 
                        || (i == model.TotalPages) 
                        || ((model.CurrentPage - i <= 3) 
                            && (model.CurrentPage - i >= - 3)))
                    {
                        anchorInnerHtml = i.ToString();
                    }
                    else if ((i == model.CurrentPage -4)  
                             || (i == model.CurrentPage + 4))
                    {
                        anchorInnerHtml = "..";
                    }

                    break;
                }
                default:
                {
                    if (model.TotalPages - model.CurrentPage < 5)
                    {
                        if ((i == 1) 
                            || (model.TotalPages  - i  <=  7))
                        {
                            anchorInnerHtml = i.ToString();
                        }
                        else if (i == model.TotalPages - 8)
                        {
                            anchorInnerHtml = "..";
                        }
                    }

                    break;
                }
            }

        }
        return anchorInnerHtml;

    }
}