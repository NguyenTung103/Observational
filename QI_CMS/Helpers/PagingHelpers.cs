using HelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            //ul tổng
            TagBuilder ul = new TagBuilder("ul");
            ul.MergeAttribute("class", "pagination");

            //li đầu tiên
            TagBuilder lifirst = new TagBuilder("li");
            TagBuilder afirst = new TagBuilder("a");
            TagBuilder ifirst = new TagBuilder("i");
            ifirst.AddCssClass("fa fa-chevron-left");

            if (pagingInfo.CurrentPage == 1)
            {
                afirst.MergeAttribute("href", "#");
                afirst.InnerHtml += ifirst;
                lifirst.InnerHtml += afirst;
                lifirst.AddCssClass("disabled");
            }
            else
            {
                afirst.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
                afirst.InnerHtml += ifirst;
                lifirst.InnerHtml += afirst;
            }
            ul.InnerHtml += lifirst;

            //khởi tạo các trang
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder liPage = new TagBuilder("li");
                TagBuilder aPage = new TagBuilder("a");
                aPage.MergeAttribute("href", pageUrl(i));
                aPage.InnerHtml = i.ToString();
                liPage.InnerHtml += aPage;
                if (i == pagingInfo.CurrentPage)
                    liPage.AddCssClass("active");
                ul.InnerHtml += liPage;
            }

            //li cuối cùng
            TagBuilder lilast = new TagBuilder("li");
            TagBuilder alast = new TagBuilder("a");
            TagBuilder ilast = new TagBuilder("i");
            ilast.AddCssClass("fa fa-chevron-right");

            if (pagingInfo.CurrentPage == pagingInfo.TotalPages || pagingInfo.TotalPages == 0)
            {
                alast.MergeAttribute("href", "#");
                alast.InnerHtml += ilast;
                lilast.InnerHtml += alast;
                lilast.AddCssClass("disabled");
            }
            else
            {
                alast.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
                alast.InnerHtml += ilast;
                lilast.InnerHtml += alast;
            }
            ul.InnerHtml += lilast;

            result.Append(ul);
            return MvcHtmlString.Create(result.ToString());
        }
    }
}