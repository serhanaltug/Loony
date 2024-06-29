using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace Loony.Web.Extensions
{
    [HtmlTargetElement("message")]
    public class MessageTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;

            StringBuilder htmlContent = new StringBuilder();

            htmlContent.Append("<div class='row'>");
            htmlContent.Append("<div class='col-lg-12'>");
            htmlContent.Append("<div class='m-alert m-alert--icon m-alert--air alert alert-primary alert-dismissible fade show' role='alert'>");
            htmlContent.Append("<div class='m-alert__icon'><i class='la la-check-circle-o'></i></div>");

            htmlContent.Append("<div class='m-alert__text'>");

            output.PreContent.SetHtmlContent(htmlContent.ToString());

            htmlContent = new StringBuilder();

            htmlContent.Append("</div>");
            htmlContent.Append("<div class='m-alert__close'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'></button></div>");
            htmlContent.Append("</div>");
            htmlContent.Append("</div>");
            htmlContent.Append("</div>");

            output.PostContent.SetHtmlContent(htmlContent.ToString());

            base.Process(context, output);
        }
    }
}
