using Loony.Web.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace Loony.Web.Extensions
{
    [HtmlTargetElement("div", Attributes = "page-data")]
    public class PageTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContext;

        public PageTagHelper(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public Page pageData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var firstDisabled = pageData.SelectedPage == 1 ? "disabled" : "";
            var lastDisabled = pageData.SelectedPage == pageData.PageCount() ? "disabled" : "";

            var prevDisabled = !pageData.HasPreviousPage ? "disabled" : "";
            var nextDisabled = !pageData.HasNextPage ? "disabled" : "";


            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<hr/>");
            htmlContent.Append("<div class='row'>");
            htmlContent.Append("<div class='col-md-6'>");
            if (pageData.DataCount > pageData.PageSize)
            {
                htmlContent.Append("<ul class='pagination'>");

                htmlContent.Append($"<li class='page-item {firstDisabled}'>");
                htmlContent.AppendFormat("<a class='page-link' href='javascript:;' onclick='GetData({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\")'><i class='fa fa-angle-double-left'></i></a>", 1, pageData.Order, pageData.SortDirection, pageData.Filter, pageData.Search);
                htmlContent.Append("</li>");

                htmlContent.Append($"<li class='page-item {prevDisabled}'>");
                htmlContent.AppendFormat("<a class='page-link' href='javascript:;' onclick='GetData({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\")'><i class='fa fa-angle-left'></i></a>", pageData.SelectedPage - 1, pageData.Order, pageData.SortDirection, pageData.Filter, pageData.Search);
                htmlContent.Append("</li>");

                for (int i = 1; i < pageData.PageCount() + 1; i++)
                {
                    if (pageData.PageCount() > 12)
                    {
                        if (i == 1 || i == pageData.PageCount())
                        {
                            htmlContent.AppendFormat("<li class='page-item {0}'>", i == pageData.SelectedPage ? "active" : "");
                            htmlContent.AppendFormat("<a class='page-link' href='javascript:;' onclick='GetData({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\")'>{0}</a>", i, pageData.Order, pageData.SortDirection, pageData.Filter, pageData.Search);
                            htmlContent.Append("</li>");
                            continue;
                        }
                        if (i >= pageData.SelectedPage - 1 && i <= pageData.SelectedPage + 9)
                        {
                            htmlContent.AppendFormat("<li class='page-item {0}'>", i == pageData.SelectedPage ? "active" : "");
                            htmlContent.AppendFormat("<a class='page-link' href='javascript:;' onclick='GetData({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\")'>{0}</a>", i, pageData.Order, pageData.SortDirection, pageData.Filter, pageData.Search);
                            htmlContent.Append("</li>");
                        }
                        else if (i == pageData.SelectedPage - 2 || i == pageData.SelectedPage + 10)
                        {
                            htmlContent.Append("<li><a>. . .</a></li>");
                        }
                    }
                    else
                    {
                        htmlContent.AppendFormat("<li class='page-item {0}'>", i == pageData.SelectedPage ? "active" : "");
                        htmlContent.AppendFormat("<a class='page-link' href='javascript:;' onclick='GetData({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\")'>{0}</a>", i, pageData.Order, pageData.SortDirection, pageData.Filter, pageData.Search);
                        htmlContent.Append("</li>");
                    }
                }

                htmlContent.Append($"<li class='page-item {nextDisabled}'>");
                htmlContent.AppendFormat("<a class='page-link' href='javascript:;' onclick='GetData({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\")'><i class='fa fa-angle-right'></i></a>", pageData.SelectedPage + 1, pageData.Order, pageData.SortDirection, pageData.Filter, pageData.Search);
                htmlContent.Append("</li>");

                htmlContent.Append($"<li class='page-item {lastDisabled}'>");
                htmlContent.AppendFormat("<a class='page-link' href='javascript:;' onclick='GetData({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\")'><i class='fa fa-angle-double-right'></i></a>", pageData.PageCount(), pageData.Order, pageData.SortDirection, pageData.Filter, pageData.Search);
                htmlContent.Append("</li>");

                htmlContent.Append("</ul>");
            }
            htmlContent.Append("</div>");
            htmlContent.Append("<div class='col-md-6 text-right'>Total: " + pageData.DataCount + "</div>");

            htmlContent.Append("</div>");

            output.Content.SetHtmlContent(htmlContent.ToString());
            base.Process(context, output);
        }
    }
}
