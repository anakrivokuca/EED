using System;
using System.Web.Mvc;

namespace EED.Ui.Web.Pagination
{
    public static class PagingHtmlHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, 
            PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            string result = String.Empty;

            for (int i = 1; i <= pagingInfo.CalculateNumberOfPages(); i++)
            {
                // Construct an <a> tag
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                    tag.AddCssClass("selected");
                result += tag.ToString();
            }
            return MvcHtmlString.Create(result);
        }
    }
}